import * as Yup from 'yup';

const LoginValidationScheme = Yup.object().shape({
    username: Yup
        .string()
        .required('\'Gebruikersnaam\' is een verplicht veld.'),
    password: Yup
        .string()
        .required('\'Wachtwoord\' is een verplicht veld.'),
});

export default LoginValidationScheme;