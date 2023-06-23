import * as Yup from 'yup';

const checkNationalInsuranceNr = (rrn) => {
    let bestaandeControleGetal = rrn.substring(9, 11);
    let deelVanStringIndienGeborenVoor2000 = rrn.substring(-1, 9);
    let getal = parseInt(deelVanStringIndienGeborenVoor2000);
    let berekendControleGetal = 97 - (getal % 97);
    if (bestaandeControleGetal == berekendControleGetal) {
        return true;
    } else {
        let deelVanStringIndienGeborenNa2000 = "2" + deelVanStringIndienGeborenVoor2000;
        getal = parseInt(deelVanStringIndienGeborenNa2000);
        berekendControleGetal = 97 - (getal % 97);
        if (bestaandeControleGetal == berekendControleGetal) {
            return true;
        } else {
            return false;
        }
    }
}

const CreateValidationScheme = Yup.object().shape({
    lastName: Yup
        .string()
        .min(1)
        .max(25, '\'Familienaam\' kan maximaal 25 karakters lang zijn.')
        .required('\'Familienaam\' is een verplicht veld.')
        .matches(/^[aA-zZ\s]+$/, '\'Familienaam\' enkel letters toegelaten.'),
    firstName: Yup
        .string()
        .min(1)
        .max(25, '\'Voornaam\' kan maximaal 25 karakters lang zijn.')
        .required('\'Voornaam\' is een verplicht veld.')
        .matches(/^[aA-zZ\s]+$/, '\'Voornaam\' enkel letters toegelaten.'),
    dateOfBirth: Yup
        .date()
        .required('\'Geboortedatum\' is een verplicht veld.'),
    driverLicenseTypeID: Yup
        .array()
        .test('check length array', '\'Rijbewijstype\' is een verplicht veld.',
            function(data) {
                return data.length >= 1
            }),
    vehicleID: Yup
        .number()
        .nullable(true),
    fuelCardID: Yup
        .number()
        .nullable(true),
    address: Yup.object({
        street: Yup
            .string()
            .min(1)
            .max(25, '\'Straat\' kan maximaal 25 karakters lang zijn.')
            .required('\'Straat\' is een verplicht veld.')
            .matches(/^[aA-zZ\s]+$/, '\'Straat\' enkel letters toegelaten.'),
        number: Yup
            .number()
            .min(1)
            .max(999)
            .required('\'Nr\' is een verplicht veld.'),
        zipcode: Yup
            .number()
            .min(1)
            .max(9999)
            .required('\'Postcode\' is een verplicht veld.'),
        place: Yup
            .string()
            .min(1)
            .max(35, '\'Plaats\' kan maximaal 25 karakters lang zijn.')
            .required('\'Plaats\' is een verplicht veld.')
            .matches(/^[aA-zZ\s]+$/, '\'plaats\' enkel letters toegelaten.'),
    }),
    nationalInsuranceNr: Yup
        .string()
        .min(11, 'Rijksregisternummer moet bestaan uit 11 cijfers.')
        .max(11, 'Rijksregisternummer moet bestaan uit 11 cijfers.')
        .required('\'Rijksregisternummer\' is een verplicht veld.')
        .test('check insurance number', 'Rijksregisternummer is ongeldig.', checkNationalInsuranceNr),
});
export default CreateValidationScheme;