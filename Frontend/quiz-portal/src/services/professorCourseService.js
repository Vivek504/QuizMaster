import axios from 'axios';

const apiUrl = window.location.origin + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const viewProfessorCourses = async (token) => {
    try {
        const response = await axios.get(
            `${apiUrl}/ProfessorCourse/getProfessorCourses`,
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