import Axios from "axios";
import {authHeader} from "../helpers/auth-header";
import {toast} from "react-toastify";
import {generalHelper} from "../helpers/general.helper";
import withReactContent from "sweetalert2-react-content";
import Swal from "sweetalert2";

const getAllFuelCards = () => {
    return Axios.get(
        `${process.env.REACT_APP_BASE_URL}fuelCard/index`,
        { 'headers': authHeader() }
        );
}

const getSelectListsFuelCard = (id) => {
    if (id){
        return Axios.get(
            `${process.env.REACT_APP_BASE_URL}fuelCard/edit/${id}`,
            { 'headers': authHeader() }
            );
    } else {
        return Axios.get(
            `${process.env.REACT_APP_BASE_URL}fuelCard/create`,
            { 'headers': authHeader() }
        );
    }
}

const createFuelCard = (values) => {
    return Axios.post(
        `${process.env.REACT_APP_BASE_URL}fuelCard/create`, values,
        { 'headers': authHeader() }
    );
}

const getFuelCard = (id) => {
    return Axios.get(
        `${process.env.REACT_APP_BASE_URL}fuelCard/details/${id}`,
        { 'headers': authHeader() }
        );
}

const editFuelCard = (values) => {
    return Axios.put(
        `${process.env.REACT_APP_BASE_URL}fuelCard/edit`, values,
        { 'headers': authHeader() }
        );
}

const deleteFuelCard = (id) => {

    const MySwal = withReactContent(Swal);

    MySwal.fire({
        icon: "question",
        text: "Ben je zeker dat je deze tankkaart wilt verwijderen?",
        confirmButtonText: "ja",
        cancelButtonText: "Nee",
        showCancelButton: true,
        confirmButtonColor: "red",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            const loadToaster = toast.loading("Aan het laden..", { type: 'info' });
            Axios.delete(
                `${process.env.REACT_APP_BASE_URL}fuelCard/remove/${id}`,
                { 'headers': authHeader() })
                .then((data) => {
                    toast.dismiss(loadToaster);
                    window.location = "/fuelCard";
                })
                .catch((error) => {
                    toast.dismiss(loadToaster);
                    toast("Er liep iets fout bij de actie. Er is niets gewijzigd.", { type: 'error' });
                });
        }
    });
};

export const fuelCardService = {
    getAllFuelCards,
    getSelectListsFuelCard,
    createFuelCard,
    getFuelCard,
    editFuelCard,
    deleteFuelCard
}