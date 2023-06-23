import * as Yup from "yup";

const CreateValidationScheme = Yup.object().shape({
  cardNumber: Yup.string()
    .max(10, '\'Kaartnummer\' mag maximaal 10 karakters bevatten.')
    .required("Kaartnummer is een verplicht veld."),
  expirationDate: Yup
      .string()
      .required("Vervaldatum is een verplicht veld."),
  pincode: Yup
      .string()
      .min(4, "Minstens 4 cijfers")
      .max(4, "Tot 4 cijfers lang")
      .matches(/^[0-9\s]+$/, '\'pincode\' enkel cijfers toegelaten.'),
  fuelTypeID: Yup
      .array()
      .test("check brandstof types", "Brandstof type is een verplicht veld",
          function(data){
              return data.length >= 1;
          }),
    driverID: Yup
        .number()
        .nullable(true)
});

export default CreateValidationScheme;
