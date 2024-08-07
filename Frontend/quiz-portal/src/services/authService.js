import axios from 'axios';

const apiUrl = `${process.env.REACT_APP_BACKEND_URL}` + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const login = async (email, password) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Auth/login`,
            {
                email: email,
                password: password
            },
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json'
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error('Login failed:', error.message);
    }
}

export const register = async (email, password, role) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Auth/register`,
            {
                email: email,
                password: password,
                role: role
            },
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json'
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error('Registration failed:', error.message);
    }
}

export const forgotPassword = async (email) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Auth/forgotPassword`,
            {
                email: email,
                resetPasswordUrl: `${window.location.origin}` + "/reset-password"
            },
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json'
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error('Forgot password failed:', error.message);
    }
}

export const resetPassword = async (code, password) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Auth/resetPassword`,
            {
                code: code,
                password: password
            },
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json'
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error('Reset password failed:', error.message);
    }
}