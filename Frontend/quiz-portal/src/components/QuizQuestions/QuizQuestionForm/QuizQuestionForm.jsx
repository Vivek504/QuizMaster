import React from "react";
import { useForm, Controller } from 'react-hook-form';
import { z, object } from 'zod';
import { zodResolver } from "@hookform/resolvers/zod";

const validationSchema = object({
    questionText: z.string().min(1, { message: 'Question is required.' }),
});

const resolver = zodResolver(validationSchema);

const QuizQuestionForm = ({ questionText, title, handler, formError, redirectToBackPage }) => {
    const { control, handleSubmit, formState: { errors }, trigger } = useForm({
        resolver,
        defaultValues: {
            questionText: questionText
        }
    });

    return (
        <div className="flex flex-col items-center justify-center mt-40">
            <article class="prose">
                <h1>{title}</h1>
            </article>
            <form className="mt-3 d w-96" onSubmit={handleSubmit(handler)}>
                <label className="input input-bordered flex items-center gap-2">
                    Question
                    <Controller
                        name="questionText"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="text"
                                className="grow"
                                error={!!errors.questionText}
                                onBlur={() => trigger('questionText')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.questionText?.message}</p>
                <p className="text-red-600 text-sm mt-3">{formError}</p>
                <div className="flex justify-between">
                    <button className="btn mt-3 w-1/3" onClick={redirectToBackPage}>Cancel</button>
                    <button className="btn mt-3 w-1/3" type="submit">Submit</button>
                </div>
            </form>
        </div>
    );
}

export default QuizQuestionForm;