import Axios from "axios";
import {authHeader} from "../helpers/auth-header";
import {toast} from "react-toastify";
import withReactContent from "sweetalert2-react-content";
import Swal from "sweetalert2";

const getAllDrivers = () => {
    return Axios.get(
        `${process.env.REACT_APP_BASE_URL}driver/index`,
        { 'headers': authHeader() }
    );
}

const getSelectListsDriver = (id) => {
    if (id){
        return Axios.get(
            `${process.env.REACT_APP_BASE_URL}driver/edit/${id}`,
            { 'headers': authHeader() })
    }else {
        return Axios.get(
            `${process.env.REACT_APP_BASE_URL}driver/create`,
            { 'headers': authHeader() })
    }
}

const createDriver = (values) => {
    return Axios.post(
        `${process.env.REACT_APP_BASE_URL}driver/create`, values,
        { 'headers': authHeader() }
    );
}

const getDriver = (id) => {
    return Axios.get(
        `${process.env.REACT_APP_BASE_URL}driver/details/${id}`,
        { 'headers': authHeader() }
        );
}

const editDriver = (values) => {
    return Axios.put(
        `${process.env.REACT_APP_BASE_URL}driver/edit`, values,
        { 'headers': authHeader() }
        );
}

const deleteDriver = (id) => {

    const MySwal = withReactContent(Swal);

    MySwal.fire({
        icon: 'question',
        text: 'Ben je zeker dat je deze bestuurder wilt verwijderen?',
        confirmButtonText: 'ja',
        cancelButtonText: 'Nee',
        showCancelButton: true,
        confirmButtonColor: 'red',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed){
            const loadToaster = toast.loading('Aan het laden..', { type: 'info' });
            Axios.delete(
                `${process.env.REACT_APP_BASE_URL}driver/remove/${id}`,
                { 'headers': authHeader() }
                )
                .then((data) => {
                    toast.dismiss(loadToaster);
                    window.location = "/driver";
                })
                .catch((error) => {
                    toast.dismiss(loadToaster);
                    toast('Er liep iets fout bij de actie. Er is niets gewijzigd.', { type: 'error' });
                });
        }
    })
}

export const driverService = {
    getAllDrivers,
    getSelectListsDriver,
    createDriver,
    getDriver,
    deleteDriver,
    editDriver
}