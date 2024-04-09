import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useForm, Controller } from 'react-hook-form';
import { z, object } from 'zod';
import { zodResolver } from "@hookform/resolvers/zod";
import { useAuth } from "../../../authUtility/authprovider";
import ROLES from "../../../constants/roles";

const validationSchema = object({
    email: z.string().email(),
    password: z.string().min(1, { message: 'Password is required.' }),
});

const resolver = zodResolver(validationSchema);

const StudentLogin = () => {
    const { control, handleSubmit, formState: { errors }, trigger } = useForm({
        resolver,
        defaultValues: {
            email: '',
            password: ''
        }
    });
    const { login } = useAuth();

    const [loginMessage, setLoginMessage] = useState();

    const onLoginRequest = async (data) => {
        const response_data = await login(data.email, data.password, ROLES.STUDENT);

        if(response_data.statusCode !== 200){
            setLoginMessage(response_data.message);
        }
    }

    return (
        <div className="flex flex-col items-center justify-center h-screen">
            <article class="prose">
                <h1>Hello Student</h1>
            </article>
            <p>Welcome back!</p>
            <form className="mt-3 d w-96" onSubmit={handleSubmit(onLoginRequest)}>
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
                <p className="text-red-600 text-sm mt-2">{loginMessage}</p>
                <div className="mt-3 flex justify-center">
                    <label className="mr-1">Are you a professor?</label>
                    <Link className="link link-primary" to="/professor/login">Login</Link>
                </div>
                <div className="mt-3 flex justify-center">
                    <label className="mr-1">Forgot password?</label>
                    <Link className="link link-primary" to="/forgot-password">Forgot Password</Link>
                </div>
                <button className="btn mt-3 w-full" type="submit">Login</button>
            </form>
        </div>
    );
}

export default StudentLogin;