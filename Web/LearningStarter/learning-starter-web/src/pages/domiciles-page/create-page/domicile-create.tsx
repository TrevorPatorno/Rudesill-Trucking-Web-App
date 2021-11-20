import "./domicile-create.css";
import axios, { AxiosError } from "axios";
import React, { useMemo } from "react";
import { Formik, Form, Field } from "formik";
import { Button } from "semantic-ui-react";
import { useHistory } from "react-router-dom";
import { ApiResponse, DomicileLocationCreateDto } from "../../../constants/types";
import { routes } from "../../../routes/config";
import { baseUrl } from "../../../constants/url-helper";


type CreateDomicileRequest = DomicileLocationCreateDto;

type CreateDomicileResponse = ApiResponse<DomicileLocationCreateDto>;

type FormValues = CreateDomicileRequest;

export const DomicileCreatePage =() => {
    const history = useHistory();

    const initialValues = useMemo<FormValues>(
    () => ({
        name: "", 
        phoneNum: "",
        streetaddress1: "",
        streetAddress2: "",
        city: "",
        state: "",
        zipCode: 0,
    }),
    []
    );

    const submitCreate = (values: CreateDomicileRequest) => {
    if (baseUrl === undefined) {
      return;
    }
    values.name = String(values.name);
    values.phoneNum = String(values.phoneNum);
    values.streetaddress1 = String(values.streetaddress1);
    values.streetAddress2 = String(values.streetAddress2);
    values.city = String(values.city);
    values.state = String(values.state);
    values.zipCode = Number(values.zipCode);

    console.log("Values: ", values);

    axios
      .post<CreateDomicileResponse>(`${baseUrl}/api/domicilelocations/create`, values) 
      .then((response) => {
        //there should be no errors here, but just in case there are errors for some unknown reason
        if (response.data.hasErrors) {
          response.data.errors.forEach((err) => {
            console.error(`${err.property}: ${err.message}`);
          });
          alert("There was an Error");
          return;
        }
        console.log("Successfully Created Domicile");
        alert("Successfully Created");
        history.push(routes.domiciles);
      })
      .catch(({ response, ...rest }: AxiosError<CreateDomicileResponse>) => {
        if (response?.data.hasErrors) {
          response?.data.errors.forEach((err) => {
            console.log(err.message);
          });
          alert(response?.data.errors[0].message);
        } else {
          alert(`There was an error creating the domicile location line 70`);
        }
        console.log(rest.toJSON());
      });
    };

return (
    <div className="flex-box-centered-content-create-class">
      <div className="create-class-form">
        <Formik initialValues={initialValues} onSubmit={submitCreate}>
          <Form>
            <div>
              <div>
                <div className="field-label">
                  <label htmlFor="name">Name</label>
                </div>
                <Field className="field" id="name" name="name" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="phoneNum">Phone Number</label>
                </div>
                <Field className="field" id="phoneNum" name="phoneNum" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="streetaddress1">Street Address 1</label>
                </div>
                <Field className="field" id="streetaddress1" name="streetaddress1" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="streetaddress2">Street Address 2</label>
                </div>
                <Field className="field" id="streetaddress2" name="streetaddress2" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="city">City</label>
                </div>
                <Field className="field" id="city" name="city" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="state">State</label>
                </div>
                <Field className="field" id="state" name="state" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="zipCode">ZipCode</label>
                </div>
                <Field className="field" id="zipCode" name="zipCode" />
              </div>
              <div className="button-container-create-class">
                <Button className="create-button" type="submit">
                  Create
                </Button>
              </div>
            </div>
          </Form>
        </Formik>
      </div>
    </div>
  );
}