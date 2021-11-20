import "./domicile-listing.css";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { Segment } from "semantic-ui-react";
import { ApiResponse, DomicileLocationGetDto } from "../../../constants/types";
import { baseUrl } from "../../../constants/url-helper";


export const DomicileListingPage = () => {
  const [domiciles, setDomiciles] = useState<ApiResponse<DomicileLocationGetDto[]>>();

  useEffect(() => {
    axios
      .get<ApiResponse<DomicileLocationGetDto[]>>(`${baseUrl}/api/domicilelocations/get`)
      .then((response) => {
        if (response.data.hasErrors) {
          response.data.errors.forEach((err) => {
            console.error(`${err.property}: ${err.message}`);
          });
        }
        setDomiciles(response.data);
      });
    //This empty array is important to ensure this only runs once on page load
    //Otherwise this will cause an infinite loop since we are setting State
  }, []);

  const domicilesToShow = domiciles?.data;
  return (
    <div className=".flex-box-centered-content-domicile-listing">
      {domicilesToShow &&
        domicilesToShow.map((x: DomicileLocationGetDto) => {
          return (
            <div className="flex-row-fill-domicile-listing">
              <Segment className="domicile-listing-segments">
                <div>{`Id: ${x.id}`}</div>
                <div>{`Name: ${x.name}`}</div>
                <div>{`Phone Number: ${x.phoneNum}`}</div>
                <div>{`Street Address 1: ${x.streetAddress1}`}</div>
                <div>{`Street Address 2: ${x.streetAddress2}`}</div>
                <div>{`City: ${x.city}`}</div>
                <div>{`State: ${x.state}`}</div>
                <div>{`Zipcode: ${x.zipCode}`}</div>
              </Segment>
            </div>
          );
        })}
    </div>
  );
};
