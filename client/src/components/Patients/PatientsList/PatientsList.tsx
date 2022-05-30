import axios from "axios";
import moment from "moment";
import React, { useEffect, useState } from "react";
import { Pagination, Table, Button, Container, Spinner } from "react-bootstrap";
import PageItem from "react-bootstrap/PageItem";
import { Link } from "react-router-dom";

const PaginationItems = (props: any) => {
  const onPaginationItemHandler = (event: any) => {
    props.onPaginationItemsClikedHandler(event.target.id);
  };

  let pages: JSX.Element[] = [];

  for (var i = 1; i <= props.totalPages; i++) {
    const item = (
      <PageItem
        onClick={onPaginationItemHandler}
        id={i.toString()}
        key={i}
        value={i}
        active={i === props.pageNumber}
      >
        {i}
      </PageItem>
    );
    pages = [...pages, item];
  }
  return <>{pages}</>;
};

const PatientsList = () => {
  const [patients, setPatients] = useState([]);
  const [totalPages, setTotalPages] = useState(0);
  const [pageNumber, setPageNumber] = useState(1);
  const [isLoading, setIsLoading] = useState(false);

  const onPaginationItemsClikedHandler = (value: any) => {
    setPageNumber(parseInt(value));
  };

  useEffect(() => {
    setIsLoading(true);
    axios
      .get("https://localhost:7215/api/Patient/GetPatients", {
        params: { pageNumber: pageNumber },
      })
      .then((response: any) => {
        return response.data;
      })
      .then((data: any) => {
        setPatients(data.data);
        setTotalPages(data.totalPages);
        setPageNumber(data.pageNumber);
      })
      .finally(() => {
        setIsLoading(false);
      });

    return () => {};
  }, [pageNumber]);

  return (
    <>
      <Link to="addPatient">
        <Button type="button">Add Patient</Button>
      </Link>
      <br />
      <br />
      <h3>Patients List</h3>
      <Container>
        {isLoading ? (
          <Spinner animation="grow" />
        ) : (
          <div>
            <Table striped bordered hover>
              <thead>
                <tr>
                  <th>Patient Name</th>
                  <th>ID Number</th>
                  <th>Date of Birth</th>
                  <th>Last Entry</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {patients.length === 0 ? (
                  <tr>
                    <td align="center" colSpan={5}>
                      <p>No Data!</p>
                    </td>
                  </tr>
                ) : (
                  patients.map((p: any, index) => (
                    <tr key={p.systemIdNumber}>
                      <td>{p.name}</td>
                      <td>{p.systemIdNumber}</td>
                      <td>{moment(p.dateOfBirth).format("DD MMM yyyy")}</td>
                      <td>{moment(p.lastEntry).format("DD MMM yyyy")}</td>
                      <td>
                        {
                          <Link to={`updatePatient/${p.systemIdNumber}`}>
                            <Button>Update</Button>
                          </Link>
                        }
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </Table>
            <Pagination>
              <PaginationItems
                totalPages={totalPages}
                pageNumber={pageNumber}
                onPaginationItemsClikedHandler={onPaginationItemsClikedHandler}
              ></PaginationItems>
            </Pagination>
          </div>
        )}
      </Container>
    </>
  );
};

export default PatientsList;
