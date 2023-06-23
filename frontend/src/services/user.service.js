import Axios from "axios";
import {toast} from "react-toastify";

const login = (values) => {
  return Axios.post(
    `${process.env.REACT_APP_BASE_URL}user/authenticate`,
    values
  )
    .then(handleResponse)
    .then((user) => {
      // store user details and jwt token in local storage to keep user logged in between page refreshes
      localStorage.setItem("user", JSON.stringify(user));

      window.location.reload();
    })
    .catch((error) => {
      toast('Gebruikersnaam of wachtwoord is incorrect.', { type: 'error' })
    });
}

const logout = () => {
  // remove user from local storage to log user out
  localStorage.removeItem("user");

  window.location.href = "/";
}

const handleResponse = (response) => {
  if (!(response.status === 200)) {
    if (response.status === 401) {
      // auto logout if 401 response returned from api
      logout();
      window.location.reload();
    }
    const error =
      (response.data && response.data.message) || response.statusText;
    return Promise.reject(error);
  }

  return response.data;
}

export const userService = {
  login,
  logout,
};
