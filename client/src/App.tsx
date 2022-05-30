import React from "react";
import "./App.css";
import { Container } from "react-bootstrap";
import Patients from "./components/Patients/Patients";
import "bootstrap/dist/css/bootstrap.min.css";

function App() {
  return (
    <Container className="p-3">
      <Patients></Patients>
    </Container>
  );
}

export default App;
