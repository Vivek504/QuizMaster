import React, { useState } from "react";
import { z, object } from 'zod';
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm, Controller } from 'react-hook-form';
import { forgotPassword } from "../../../services/authService";
import { useNavigate } from "react-router-dom";

const validationSchema = object({
    email: z.string().email()
});
const resolver = zodResolver(validationSchema);

const ForgotPassword = () => {
    const { control, handleSubmit, formState: { errors }, trigger } = useForm({
        resolver,
        defaultValues: {
            email: ''
        }
    });

    const [forgotPasswordMessaget, setForgotPasswordMessage] = useState();

    const navigate = useNavigate();

    const onForgotPasswordRequest = async (data) => {
        const response_data = await forgotPassword(data.email);

        if(response_data.statusCode !== 200){
            setForgotPasswordMessage(response_data.message);
        }
    }

    const redirectToBackPage = () => {
        navigate('/');
    }

    return (
        <div className="flex flex-col items-center justify-center h-screen">
            <article class="prose">
                <h1>Forgot Password</h1>
            </article>
            <p>We've got you covered</p>
            <form className="mt-3 d w-96" onSubmit={handleSubmit(onForgotPasswordRequest)}>
                <label className="input input-bordered flex items-center gap-2 mt-3">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" fill="currentColor" className="w-4 h-4 opacity-70"><path d="M2.5 3A1.5 1.5 0 0 0 1 4.5v.793c.026.009.051.02.076.032L7.674 8.51c.206.1.446.1.652 0l6.598-3.185A.755.755 0 0 1 15 5.293V4.5A1.5 1.5 0 0 0 13.5 3h-11Z" /><path d="M15 6.954 8.978 9.86a2.25 2.25 0 0 1-1.956 0L1 6.954V11.5A1.5 1.5 0 0 0 2.5 13h11a1.5 1.5 0 0 0 1.5-1.5V6.954Z" /></svg>
                    <Controller
                        name="email"
                        control={control}
                        render={({ field }) => (
                            <input
                                {...field}
                                type="text"
                                className="grow"
                                placeholder="email"
                                error={!!errors.email}
                                onBlur={() => trigger('email')}
                            />
                        )}
                    />
                </label>
                <p className="text-red-600 text-sm">{errors.email?.message}</p>
                <p className="text-red-600 text-sm mt-2">{forgotPasswordMessaget}</p>
                <div className="flex justify-between">
                    <button className="btn mt-3 w-1/3" onClick={redirectToBackPage}>Cancel</button>
                    <button className="btn mt-3 w-1/3" type="submit">Submit</button>
                </div>
            </form>
        </div>
    );
}

export default ForgotPassword;