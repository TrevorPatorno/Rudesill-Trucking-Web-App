import "./login-page.css";
import axios, { AxiosError } from "axios";
import React, { useMemo } from "react";
import { ApiResponse } from "../../constants/types";
import { Formik, Form, Field } from "formik";
import { Button } from "semantic-ui-react";
import { PageWrapper } from "../../components/page-wrapper/page-wrapper";
import { loginUser } from "../../authentication/authentication-services";
import { baseUrl } from "../../constants/url-helper";


type LoginRequest = {
  userName: string;
  password: string;
};

type LoginResponse = ApiResponse<boolean>;

type FormValues = LoginRequest;

//This is a *fairly* basic form
//The css used in here is a good example of how flexbox works in css
//For more info on flexbox: https://css-tricks.com/snippets/css/a-guide-to-flexbox/
export const LoginPage = () => {
  const initialValues = useMemo<FormValues>(
    () => ({
      userName: "",
      password: "",
    }),
    []
  );

  const submitLogin = (values: LoginRequest) => {
    axios
      .post<LoginResponse>(`${baseUrl}/api/authenticate`, values)
      .then((response) => {
        if (response.data.data) {
          console.log("Successfully Logged In!");
          loginUser();
        }
      })
      .catch(({ response, ...rest }: AxiosError<LoginResponse>) => {
        if (response?.data.hasErrors) {
          response?.data.errors.forEach((err) => {
            console.log(err.message);
          });
          alert(response?.data.errors[0].message);
        } else {
          alert(`There was an error logging in`);
        }
        console.log(rest.toJSON());
      });
  };

  return (
    <PageWrapper>
      <div className="flex-box-centered-content-login-page">
        <div className="login-form">
          <Formik initialValues={initialValues} onSubmit={submitLogin}>
            <Form>
              <div>
                <div>
                  <div className="field-label">
                    <label htmlFor="userName">UserName</label>
                  </div>
                  <Field className="field" id="userName" name="userName" />
                </div>
                <div>
                  <div className="field-label">
                    <label htmlFor="password">Password</label>
                  </div>
                  <Field className="field" id="password" name="password" type= "password" />
                </div>
                <div className="button-container-login-page">
                  <Button className="login-button" type="submit">
                    Login
                  </Button>
                </div>
              </div>
            </Form>
          </Formik>
        </div>
      </div>
    </PageWrapper>
  );
};
