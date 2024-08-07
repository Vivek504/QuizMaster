import axios from 'axios';

const apiUrl = `${process.env.REACT_APP_BACKEND_URL}` + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const addStudent = async (token, email, courseId) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Student/addStudent`,
            {
                email,
                courseId
            },
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error(error.message);
    }
}