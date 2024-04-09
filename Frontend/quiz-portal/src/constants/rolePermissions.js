import PERMISSIONS from "./permissions";
import ROLES from "./roles";

export const ROLE_PERMISSIONS = {
    [ROLES.PROFESSOR]: [
        PERMISSIONS.ADDCOURSE,
        PERMISSIONS.UPDATECOURSE,
        PERMISSIONS.STUDENTS
    ],
    [ROLES.STUDENT]: [

    ]
};  

export default ROLE_PERMISSIONS;