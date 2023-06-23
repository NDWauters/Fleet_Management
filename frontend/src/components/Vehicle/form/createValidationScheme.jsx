import * as Yup from 'yup';

const CreateValidationScheme = Yup.object().shape({

    brandID: Yup
        .number()
        .required('\'Merk\' is een verplicht veld.'),
    fuelTypeID: Yup
        .number()
        .required('\'Brandstoftype\' is een verplicht veld.'),
    vehicleTypeID: Yup
        .number()
        .required('\'Voertuigtype\' is een verplicht veld.'),
    licensePlate: Yup
        .string()
        .max(7, '\'Nummerplaat\' mag maximaal 7 karakters lang zijn.')
        .required('\'Nummerplaat\' is een verplicht veld.'),
    vin: Yup
        .string()
        .max(17, '\'Chassisnummer\' mag maximaal 17 karakters lang zijn.')
        .required('\'Chassisnummer\' is een verplicht veld.'),
    model: Yup
        .string()
        .max(150, '\'Model\' mag maximaal 150 karakters lang zijn.')
        .required('\'Model\' is een verplicht veld.'),
    color: Yup
        .string()
        .max(150, '\'Kleur\' mag maximaal 150 karakters lang zijn.'),
    amountDoors: Yup
        .number()
        .typeError('\'Aantal deuren\' moet een getal zijn.'),
    driverID: Yup
        .number()
        .nullable(true)
});

export default CreateValidationScheme;