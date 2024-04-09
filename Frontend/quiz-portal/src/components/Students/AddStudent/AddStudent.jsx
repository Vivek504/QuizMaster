import React, { useState } from "react";
import { useParams } from "react-router-dom";
import Navbar from "../../Navbar/Navbar";
import StudentForm from "../StudentForm/StudentForm";
import { addStudent as addStudentService } from "../../../services/studentService";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import { useNavigate } from "react-router-dom";

const AddStudent = () => {
    const { id } = useParams();
    const [formError, setFormError] = useState();

    const navigate = useNavigate();

    const addStudent = async (data) => {
        const response_data = await addStudentService(localStorage.getItem(LocalStorageKey.JWTTOKEN), data.email, id);

        if(response_data.responseCode === 200){
            redirectToBackPage();
        }
        else{
            setFormError(response_data.message);
        }
    }

    const redirectToBackPage = () => {
        navigate(`/course/${id}/students`);
    }

    return (
        <div>
            <Navbar />
            <StudentForm title="Add Student" handler={addStudent} formError={formError} redirectToBackPage={redirectToBackPage} email=''/>
        </div>
    );
}

export default AddStudent;