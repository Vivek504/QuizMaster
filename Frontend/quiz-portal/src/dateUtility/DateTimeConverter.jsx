export const originalToFormatted = (date) => {
    const originalDate = new Date(date);

    const formattedDate = originalDate.toLocaleString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        hour12: true,
    });
    return formattedDate;
};

export const combineDateAndTime = (date, time) => {
    const combinedDateTimeString = date + 'T' + time + ':00';

    const dateTime = new Date(combinedDateTimeString + 'Z');

    const formattedDate = dateTime.toISOString();

    return formattedDate;
};

export const extractDateAndTime = (formattedDate) => {
    const dateTime = new Date(formattedDate);

    const date = dateTime.toISOString().split('T')[0];
    const formattedTime = dateTime.toISOString().split('T')[1].slice(0, -5);

    const [hours, minutes] = formattedTime.split(':');
    const time = `${hours}:${minutes}`;
    
    return { date, time };
};

export const compareDateAndTime = (startDateTime, endDateTime) => {
    const currentDate = new Date();
    const start = new Date(startDateTime);
    const end = new Date(endDateTime);
    
    return currentDate >= start && currentDate <= end;
};