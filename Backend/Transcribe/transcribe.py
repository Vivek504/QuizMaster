import os
import uuid
import speech_recognition as sr
from moviepy.editor import VideoFileClip
import boto3
import json

region_name = os.getenv('region_name')
aws_access_key_id = os.getenv('aws_access_key_id')
aws_secret_access_key = os.getenv('aws_secret_access_key')
aws_session_token = os.getenv('aws_session_token')

s3 = boto3.client('s3', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)
sqs = boto3.client('sqs', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)

def extract_speech(body):
    print(body)
    bucket_name = body.get('BucketName')
    object_name = body.get('ObjectName')
    video_file_name = object_name
    s3.download_file(bucket_name, object_name, video_file_name)

    video  = VideoFileClip(video_file_name)
    audio = video.audio

    audio_file_name = str(uuid.uuid4()) + ".wav"
    audio.write_audiofile(audio_file_name)

    r = sr.Recognizer()

    with sr.AudioFile(audio_file_name) as source:
        audio_text = r.record(source)
    text = r.recognize_google(audio_text, language='en-US')

    os.remove(video_file_name)
    os.remove(audio_file_name)

    return text

def send_response(transcribe_response_queue_url, message):
    sqs.send_message(
        QueueUrl=transcribe_response_queue_url,
        MessageBody=json.dumps(message)
    )

def process_message(message, transcribe_response_queue_url):
    print(message)
    body = json.loads(message['Body'])
    text = extract_speech(body)
    print(text)
    send_response(transcribe_response_queue_url, {'Text': text, 'VideoId': body.get('VideoId'), 'MessageType': 'TRANSCRIBE_RESPONSE', 'IsLastAnswer': body.get('IsLastAnswer')})

def create_queue_if_not_exists(queue_name):
    response = sqs.list_queues(QueueNamePrefix=queue_name)
    if 'QueueUrls' in response:
        for url in response['QueueUrls']:
            if queue_name in url:
                return url

    response = sqs.create_queue(QueueName=queue_name)
    return response['QueueUrl']


def listen_for_messages(queue_url, transcribe_response_queue_url):
    queue_url = create_queue_if_not_exists(queue_url)
    while True:
        response = sqs.receive_message(
            QueueUrl=queue_url,
            AttributeNames=['All'],
            MaxNumberOfMessages=1,
            WaitTimeSeconds=20
        )

        if 'Messages' in response:
            for message in response['Messages']:
                process_message(message, transcribe_response_queue_url)
                sqs.delete_message(
                    QueueUrl=queue_url,
                    ReceiptHandle=message['ReceiptHandle']
                )

if __name__ == '__main__':
    transcribe_queue_url = 'transcribe-request-queue'
    transcribe_response_queue_url = 'transcribe-response-queue'
    listen_for_messages(transcribe_queue_url, transcribe_response_queue_url)
