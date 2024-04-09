import React from "react";
import { useForm, Controller } from 'react-hook-form';
import { z, object } from 'zod';
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";

const validationSchema = object({
    courseName: z.string().min(1, { message: 'Course name is required.' }),
});

const resolver = zodResolver(validationSchema);

const CourseForm = ({ courseName, title, handler, formError }) => {
    const { control, handleSubmit, formState: { errors }, trigger } = useForm({
        resolver,
        defaultValues: {
            courseName: courseName
        }
    });

    const navigate = useNavigate();

    const redirectToBackPage = () => {
        navigate(`/courses`);
    }

    return (
        <div className="flex flex-col items-center justify-center mt-40">
            <article class="prose">
                <h1>{title}</h1>
            </article>
            <form className="mt-3 d w-96" onSubmit={handleSubmit(handler)}>
                <label className="input input-bordered flex items-center gap-2">
                    Course Name
                    <Controller
                        name="courseName"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="text"
                                className="grow"
                                error={!!errors.courseName}
                                onBlur={() => trigger('courseName')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.courseName?.message}</p>
                <p className="text-red-600 text-sm mt-3">{formError}</p>
                <div className="flex justify-between">
                    <button className="btn mt-3 w-1/3" onClick={redirectToBackPage}>Cancel</button>
                    <button className="btn mt-3 w-1/3" type="submit">Submit</button>
                </div>
            </form>
        </div>
    );
}

export default CourseForm;