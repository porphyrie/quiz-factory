import React, { useEffect, useState } from "react";
import { Button, Container, Form, FormControl, FormGroup, Row, Stack, Table, ToggleButton, ToggleButtonGroup } from "react-bootstrap";
import { createAPIEndpoint, ENDPOINTS } from "../../helpers/API";

export default function AddSubjects() {

    const [isSubjectSubmitted, setIsSubjectSubmitted] = useState(0);

    const [subject, setSubject] = useState('');

    const handleSubjectSubmit = e => {
        e.preventDefault();

        createAPIEndpoint(ENDPOINTS.subjects)
            .authPost({ subjectName: subject })
            .then(res => {
                alert(res.data.message);
            })
            .catch(err => alert(err));

        setIsSubjectSubmitted(isSubjectSubmitted + 1);
    };

    const [subjects, setSubjects] = useState([]);

    useEffect(() => {
        createAPIEndpoint(ENDPOINTS.subjects)
            .authFetch()
            .then(res => {
                setSubjects(res.data);
            })
            .catch(err => alert(err));
    }, [isSubjectSubmitted]);

    return (
        <Container className='bg-violet-300 p-10 space-y-5'>
            <Row className='pb-10'>
                <h2 className='font-bold text-center'>Subiecte</h2>
            </Row>
            <Row>
                <Form onSubmit={handleSubjectSubmit}>
                    <h4 className='font-bold'>Adaugă un subiect</h4>
                    <Stack direction="horizontal" className='space-x-5'>
                        <FormControl required type='text' onChange={(e) => setSubject(e.target.value)} placeholder="Introdu denumirea subiectului" />
                        <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto'>Adaugă</Button>
                    </Stack>
                </Form>
            </Row>
            <Row>
                <h4 className='font-bold'>Vizualizează subiectele adăugate</h4>
                <div className='max-h-64 overflow-auto'>
                    <Table responsive striped bordered hover className='bg-white'>
                        <thead className='sticky-top'>
                            <tr>
                                <th>Denumire</th>
                            </tr>
                        </thead>
                        <tbody>
                            {subjects.length ?
                                subjects.map((subject) => (
                                    <tr>
                                        <td>{subject.subjectName}</td>
                                    </tr>
                                ))
                                : <tr>
                                    <td>Nu a fost adăugat niciun subiect.</td>
                                </tr>}
                        </tbody>
                    </Table>
                </div>
            </Row>
        </Container >
    )
}