import React from "react";
import { Container } from "react-bootstrap";
import { Route, Routes } from "react-router-dom";
import AddUpdatePatient from "./AddUpdatePatient/AddUpdatePatient";
import PatientsList from "./PatientsList/PatientsList";

const Patients = () => {
  return (
    <Container className="pb-1 p-5 mb-4 bg-light rounded-3">
      <h1 className="header">Patients !</h1>
      <Routes>
        <Route path="/" element={ <PatientsList/>} ></Route>
        <Route path="addPatient" element={<AddUpdatePatient />} ></Route>
        <Route path="updatePatient/:systemIdNumber" element={<AddUpdatePatient />} ></Route>
      </Routes>
    </Container>
  );
};

export default Patients;
