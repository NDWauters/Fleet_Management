import Axios from "axios";
import {authHeader} from "../helpers/auth-header";
import {toast} from "react-toastify";
import withReactContent from "sweetalert2-react-content";
import Swal from "sweetalert2";

const getAllVehicles = () => {
    return Axios.get(
        `${process.env.REACT_APP_BASE_URL}vehicle/index`,
        { 'headers': authHeader() }
    );
}

const getSelectListsVehicle = (id) => {
    if (id){
    return Axios.get(
        `${process.env.REACT_APP_BASE_URL}vehicle/edit/${id}`,
        { 'headers': authHeader() }
        );
    } else {
        return Axios.get(
            `${process.env.REACT_APP_BASE_URL}vehicle/create`,
            { 'headers': authHeader() }
            );
    }
}

const createVehicle = (values) => {
    return Axios.post(
        `${process.env.REACT_APP_BASE_URL}vehicle/create`, values,
        { 'headers': authHeader() }
        );
}

const getVehicle = (id) => {
    return Axios.get(
        `${process.env.REACT_APP_BASE_URL}vehicle/details/${id}`,
        { 'headers': authHeader() }
        );
}

const editVehicle = (values) => {
 return  Axios.put(
     `${process.env.REACT_APP_BASE_URL}vehicle/edit`, values,
     { 'headers': authHeader() }
     );
}

const deleteVehicle = (id) => {

    const MySwal = withReactContent(Swal);

    MySwal.fire({
        icon: "question",
        text: "Ben je zeker dat je dit voertuig wilt verwijderen?",
        confirmButtonText: "ja",
        cancelButtonText: "Nee",
        showCancelButton: true,
        confirmButtonColor: "red",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            const loadToaster = toast.loading("Aan het laden..", { type: 'info' });
            Axios.delete(
                `${process.env.REACT_APP_BASE_URL}vehicle/remove/${id}`,
                { 'headers': authHeader() }
                )
                .then((data) => {
                    toast.dismiss(loadToaster);
                    window.location = "/vehicle";
                })
                .catch((error) => {
                    toast.dismiss(loadToaster);
                    toast("Er liep iets fout bij de actie. Er is niets gewijzigd.", { type: 'error' });
                });
        }
    });
}

export const vehicleService = {
    getAllVehicles,
    getSelectListsVehicle,
    createVehicle,
    getVehicle,
    editVehicle,
    deleteVehicle
}