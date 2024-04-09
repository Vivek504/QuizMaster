import React from "react";
import { useForm, Controller } from 'react-hook-form';
import { z, object, ZodError } from 'zod';
import { zodResolver } from "@hookform/resolvers/zod";
import QUIZ_TYPES from "../../../constants/QuizTypes";

const validationSchema = object({
    quizTitle: z.string().min(1, { message: 'Quiz title is required.' }),
    startDate: z.string().min(1, { message: 'Start date is required.' }),
    startTime: z.string().min(1, { message: 'Start time is required.' }),
    endDate: z.string().min(1, { message: 'End date is required.' }),
    endTime: z.string().min(1, { message: 'End time is required.' }),
    quizType: z.string().min(1, { message: 'Quiz type is required.' }),
});

const resolver = zodResolver(validationSchema);

const CourseQuizForm = ({ quizTitle, startDate, startTime, endDate, endTime, quizType, title, handler, formError, redirectToBackPage }) => {
    const { control, handleSubmit, formState: { errors }, trigger } = useForm({
        resolver,
        defaultValues: {
            quizTitle: quizTitle,
            startDate: startDate,
            startTime: startTime,
            endDate: endDate,
            endTime: endTime,
            quizType: quizType
        }
    });

    return (
        <div className="flex flex-col items-center justify-center mt-40">
            <article class="prose">
                <h1>{title}</h1>
            </article>
            <form className="mt-3 d w-96" onSubmit={handleSubmit(handler)}>
                <label className="input input-bordered flex items-center gap-2">
                    Title
                    <Controller
                        name="quizTitle"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="text"
                                className="grow"
                                error={!!errors.quizTitle}
                                onBlur={() => trigger('quizTitle')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.quizTitle?.message}</p>
                <label className="input input-bordered flex items-center gap-2 mt-2">
                    Start Date
                    <Controller
                        name="startDate"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="date"
                                className="grow"
                                error={!!errors.startDate}
                                onBlur={() => trigger('startDate')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.startDate?.message}</p>
                <label className="input input-bordered flex items-center gap-2 mt-2">
                    Start Time
                    <Controller
                        name="startTime"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="time"
                                className="grow"
                                error={!!errors.startTime}
                                onBlur={() => trigger('startTime')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.startTime?.message}</p>
                <label className="input input-bordered flex items-center gap-2 mt-2">
                    End Date
                    <Controller
                        name="endDate"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="date"
                                className="grow"
                                error={!!errors.endDate}
                                onBlur={() => trigger('endDate')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.endDate?.message}</p>
                <label className="input input-bordered flex items-center gap-2 mt-2">
                    End Time
                    <Controller
                        name="endTime"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="time"
                                className="grow"
                                error={!!errors.endTime}
                                onBlur={() => trigger('endTime')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.endTime?.message}</p>
                <label className="input input-bordered flex items-center gap-2 mt-2">
                    Quiz Type
                    <Controller
                        name="quizType"
                        control={control}
                        render={({ field }) => (
                            <select
                                {...field}
                                className="grow"
                                error={!!errors.quizType}
                                onBlur={() => trigger('quizType')}
                            >
                                <option value=""></option>
                                {Object.values(QUIZ_TYPES).map(type => (
                                    <option key={type} value={type}>
                                        {type}
                                    </option>
                                ))}
                            </select>
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.quizType?.message}</p>
                <p className="text-red-600 text-sm mt-3">{formError}</p>
                <div className="flex justify-between">
                    <button className="btn mt-3 w-1/3" onClick={redirectToBackPage}>Cancel</button>
                    <button className="btn mt-3 w-1/3" type="submit">Submit</button>
                </div>
            </form>
        </div>
    );
}

export default CourseQuizForm;