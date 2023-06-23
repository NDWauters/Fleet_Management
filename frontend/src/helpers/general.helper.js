import {userService} from "../services/user.service";
import {toast} from "react-toastify";

const handleError = (error) => {

      if(error.response.status == 401){
        userService.logout();
      }
      if (error.response) {
        // The request was made and the server responded with a status code
        // that falls out of the range of 2xx
        toast('Er liep iets fout bij het laden van de pagina. Probeer opnieuw.', { type: 'error' });
      } else if (error.request) {
        // The request was made but no response was received
        // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
        // http.ClientRequest in node.js
        toast('Er liep iets fout bij het verwerken van de gegevens. Probeer opnieuw.', { type: 'error' });
      } else {
        // Something happened in setting up the request that triggered an Error
        toast('Er liep iets fout bij het laden van de pagina. Probeer opnieuw.', { type: 'error' });
      }

    return;
};

const convertMultiSelectValues = (neededValues, selectlist) => {
    const chosenOptions = selectlist.filter((e) =>
        neededValues.includes(parseInt(e.value))
    );
    const convertedValues = chosenOptions.map((e) => {
        return { label: e.text, value: e.value };
    });
    return convertedValues;
};

const convertSelectListValue = (value) => {
    if (!value){
        return null;
    }
    return value;
}

export const generalHelper = {
    handleError,
    convertMultiSelectValues,
    convertSelectListValue
}