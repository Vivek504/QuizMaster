import React, { useState } from "react";
import { z, object } from 'zod';
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm, Controller } from 'react-hook-form';
import { resetPassword } from "../../../services/authService";
import { useNavigate, useParams } from "react-router-dom";

const validationSchema = object({
    password: z.string().min(8, { message: 'Password must contain at least 8 characters.' }),
    confirmPassword: z.string(),
})
    .refine((value) => value.confirmPassword === value.password, {
        message: "Password and Confirm Password don't match.",
        path: ['confirmPassword'],
    });

const resolver = zodResolver(validationSchema);

const ResetPassword = () => {
    const { code } = useParams();

    const { control, handleSubmit, formState: { errors }, trigger } = useForm({
        resolver,
        defaultValues: {
            password: 'Prof@123',
            confirmPassword: 'Prof@123',
        },
    });

    const navigate = useNavigate();

    const [resetPasswordMessage, setResetPasswordMessage] = useState();

    const onResetPasswordRequest = async (data) => {
        const response_data = await resetPassword(code, data.password);

        if(response_data.responseCode !== 200){
            setResetPasswordMessage(response_data.message);
        }
        else{
            navigate('/');
        }
    }

    return (
        <div className="flex flex-col items-center justify-center h-screen">
            <article class="prose">
                <h1>Reset Password</h1>
            </article>
            <p>Choose a new password.</p>
            <form className="mt-3 d w-96" onSubmit={handleSubmit(onResetPasswordRequest)}>
                <label className="input input-bordered flex items-center gap-2 mt-3">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" fill="currentColor" className="w-4 h-4 opacity-70"><path fillRule="evenodd" d="M14 6a4 4 0 0 1-4.899 3.899l-1.955 1.955a.5.5 0 0 1-.353.146H5v1.5a.5.5 0 0 1-.5.5h-2a.5.5 0 0 1-.5-.5v-2.293a.5.5 0 0 1 .146-.353l3.955-3.955A4 4 0 1 1 14 6Zm-4-2a.75.75 0 0 0 0 1.5.5.5 0 0 1 .5.5.75.75 0 0 0 1.5 0 2 2 0 0 0-2-2Z" clipRule="evenodd" /></svg>
                    <Controller
                        name="password"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="password"
                                className="grow"
                                placeholder="password"
                                error={!!errors.password}
                                onBlur={() => trigger('password')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.password?.message}</p>
                <label className="input input-bordered flex items-center gap-2 mt-3">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" fill="currentColor" className="w-4 h-4 opacity-70"><path fillRule="evenodd" d="M14 6a4 4 0 0 1-4.899 3.899l-1.955 1.955a.5.5 0 0 1-.353.146H5v1.5a.5.5 0 0 1-.5.5h-2a.5.5 0 0 1-.5-.5v-2.293a.5.5 0 0 1 .146-.353l3.955-3.955A4 4 0 1 1 14 6Zm-4-2a.75.75 0 0 0 0 1.5.5.5 0 0 1 .5.5.75.75 0 0 0 1.5 0 2 2 0 0 0-2-2Z" clipRule="evenodd" /></svg>
                    <Controller
                        name="confirmPassword"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="password"
                                className="grow"
                                placeholder="confirmPassword"
                                error={!!errors.confirmPassword}
                                onBlur={() => trigger('confirmPassword')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.confirmPassword?.message}</p>
                <p className="text-red-600 text-sm mt-2">{resetPasswordMessage}</p>
                <button className="btn mt-3 w-full" type="submit">Submit</button>
            </form>
        </div>
    );
}

export default ResetPassword;