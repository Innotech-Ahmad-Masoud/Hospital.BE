import axios from "axios";
import React, { useEffect, useState } from "react";
import { Button, Form, Spinner } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";

const AddUpdatePatient = () => {
  const { systemIdNumber } = useParams();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [patientInput, setPatientInput] = useState({
    systemIdNumber: systemIdNumber,
    name: "",
    officialIDNumber: "",
    dateOfBirth: Date.now(),
    emailAddress: "",
  });

  useEffect(() => {
    if (systemIdNumber !== "") {
      axios
        .get(`https://localhost:7215/api/Patient/GetPatient/${systemIdNumber}`)
        .then((response) => {
          setPatientInput({
            systemIdNumber: response.data.data.systemIdNumber,
            name: response.data.data.name,
            officialIDNumber: response.data.data.officialIdNumber,
            dateOfBirth: response.data.data.dateOfBirth,
            emailAddress: response.data.data.emailAddress,
          });
        })
        .catch(function (error) {
          alert(error);
          console.log(error);
        });
    }
    return () => {};
  }, [systemIdNumber]);

  const nameChangeHanler = (event: any) => {
    setPatientInput(prevState =>({
      ...prevState,
      name: event.target.value,
    }));
  };

  const officialIDNumberChangeHanler = (event: any) => {
    setPatientInput({
      ...patientInput,
      officialIDNumber: event.target.value,
    });
  };

  const dateOfBirthChangeHanler = (event: any) => {
    setPatientInput({
      ...patientInput,
      dateOfBirth: event.target.value,
    });
  };

  const emailAddressChangeHanler = (event: any) => {
    setPatientInput({
      ...patientInput,
      emailAddress: event.target.value,
    });
  };

  const onSubmitHandler = (event: any) => {
    event.preventDefault();
    setIsLoading(true);
    submitPatient();
  };

  const submitPatient = () => {
    let url = "https://localhost:7215/api/Patient";
    url = patientInput.systemIdNumber
      ? `${url}/UpdatePatient`
      : `${url}/CreatePatient`;

    axios
      .post(url, patientInput)
      .then(function (response) {
        setPatientInput({
          systemIdNumber: systemIdNumber,
          name: "",
          officialIDNumber: "",
          dateOfBirth: Date.now(),
          emailAddress: "",
        });
        navigate("/");
      })
      .catch(function (error) {
        alert(error);
        console.log(error);
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  return (
    <>
      <br />
      <h3>{systemIdNumber ? "Update Patient" : "Add New Patient"}</h3>
      <Form onSubmit={onSubmitHandler}>
        <Form.Group className="mb-3" controlId="formName">
          <Form.Label>Patient Name</Form.Label>
          <Form.Control
            name="name"
            type="text"
            placeholder="Patient Name"
            onChange={nameChangeHanler}
            required
            disabled={isLoading}
            value={patientInput.name}
          />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formOfficialIDNumber">
          <Form.Label>Official ID Number</Form.Label>
          <Form.Control
            name="officialIDNumber"
            type="text"
            placeholder="Official ID Number"
            onChange={officialIDNumberChangeHanler}
            required
            disabled={isLoading || systemIdNumber === ""}
            value={patientInput.officialIDNumber}
          />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formDateOfBirth">
          <Form.Label>Date Of Birth</Form.Label>
          <Form.Control
            name="dateOfBirth"
            type="date"
            placeholder="Date Of Birth"
            onChange={dateOfBirthChangeHanler}
            disabled={isLoading}
            value={patientInput.dateOfBirth}
          />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formEmailAddress">
          <Form.Label>Email Address</Form.Label>
          <Form.Control
            name="emailAddress"
            type="email"
            placeholder="Email Address"
            onChange={emailAddressChangeHanler}
            disabled={isLoading}
            value={patientInput.emailAddress}
          />
        </Form.Group>
        <Button variant="primary" type="submit" disabled={isLoading}>
          {isLoading ? (
            <>
              <Spinner
                as="span"
                animation="border"
                size="sm"
                role="status"
                aria-hidden="true"
              />
              <span className="visually-hidden">Loading...</span>
            </>
          ) : 
            systemIdNumber ? "Update Patient" : "Add New Patient"
          }
        </Button>{" "}
        <Button
          variant="secondary"
          type="button"
          onClick={() => navigate(-1)}
          disabled={isLoading}
        >
          Cancel
        </Button>
      </Form>
    </>
  );
};

export default AddUpdatePatient;
