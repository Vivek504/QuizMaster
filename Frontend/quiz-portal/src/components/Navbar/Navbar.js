import React from "react";
import { useAuth } from "../../authUtility/authprovider";
import { useNavigate } from "react-router-dom";
import LocalStorageKey from "../../constants/LocalStorageKey";
import ROLES from "../../constants/roles";

const Navbar = () => {
    const { logout } = useAuth();
    const navigate = useNavigate();

    const onAddRedirect = () => {
        if(localStorage.getItem(LocalStorageKey.ROLE) === ROLES.PROFESSOR){
            navigate('/courses');
        }
        else{
            navigate('/student-courses');
        }
    }

    const logoutRequest = () => {
        logout();
    }

    return (
        <>
            <div className="navbar bg-base-100 justify-end">
                <button className="btn btn-ghost text-xl" onClick={onAddRedirect}>Courses</button>
                <button className="btn btn-ghost text-xl" onClick={logoutRequest}>Logout</button>
            </div>
        </>
    );
}

export default Navbar;