import axios from 'axios';

const apiUrl = `${process.env.REACT_APP_BACKEND_URL}` + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const getCourse = async (token, id) => {
    try {
        const response = await axios.get(
            `${apiUrl}/Course/getCourse?Id=${id}`,
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

export const addCourse = async (token, courseName) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Course/addCourse`,
            {
                courseName
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

export const updateCourse = async (token, id, courseName) => {
    try {
        const response = await axios.put(
            `${apiUrl}/Course/updateCourse`,
            {
                id,
                courseName
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