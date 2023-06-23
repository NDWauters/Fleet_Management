import React from 'react';
import { userService } from '../../services/user.service';
import { Form, Formik } from 'formik';
import TextBoxField from '../../components/forms/TextBoxField';
import LoginValidationScheme from '../../components/authentication/LoginValidationScheme';
import { useNavigate } from 'react-router-dom';
import {ToastContainer} from "react-toastify";

const LoginPage = () => {
  const onSubmit = (values) => {
    if (values.username && values.password) {
      userService.login(values);
    }
  };

  return (
    <div className="row mt-5">
      <div className="col-md-4"></div>
      <div className="col-md-4">
        <h2>Login</h2>
        <Formik
          initialValues={{
            username: '',
            password: '',
          }}
          onSubmit={(values) => onSubmit(values)}
          validationSchema={LoginValidationScheme}
        >
          {({ errors }) => (
            <Form>
              <div className="row">
                <div className="col-md-12">
                  <div className="row">
                    <div className="col-md-12">
                      <TextBoxField
                        name="username"
                        label="Gebruikersnaam"
                        required={true}
                      />
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-12">
                      <TextBoxField
                        name="password"
                        type="password"
                        label="Wachtwoord"
                        required={true}
                      />
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-12">
                      <hr />
                      <button
                        className="btn btn-primary float-end"
                        type="submit"
                      >
                        Log in
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </div>
      <div className="col-md-4"></div>
    <ToastContainer
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
    />
    </div>
  );
};

export default LoginPage;
