import axios from 'axios';

const apiUrl = `${process.env.REACT_APP_BACKEND_URL}` + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const viewCourseStudents = async (token, courseId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/StudentCourse/getCourseStudents?courseId=${courseId}`,
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

export const viewStudentCourses = async (token) => {
    try {
        const response = await axios.get(
            `${apiUrl}/StudentCourse/getStudentCourses`,
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

export const getStudentCourse = async (token, studentId, courseId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/StudentCourse/getStudentCourse?studentId=${studentId}&courseId=${courseId}`,
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