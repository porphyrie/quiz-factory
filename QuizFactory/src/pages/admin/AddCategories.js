import React, { useEffect, useState } from "react";
import { Button, Container, Form, FormControl, FormGroup, Row, Stack, Table, ToggleButton, ToggleButtonGroup } from "react-bootstrap";
import { createAPIEndpoint, ENDPOINTS } from "../../helpers/API";

export default function AddCategories() {

    const [subjects1, setSubjects1] = useState([]);

    const [subject1, setSubject1] = useState({});

    useEffect(() => {
        createAPIEndpoint(ENDPOINTS.subjects)
            .authFetch()
            .then(res => {
                setSubjects1(res.data);
                setSubject1(res.data[0]);
            })
            .catch(err => alert(err));
    }, []);

    const [categories1, setCategories1] = useState([])

    useEffect(() => {
        if (subject1) {
            console.log("haha");
            if (Object.keys(subject1).length) {
                createAPIEndpoint(ENDPOINTS.categories)
                    .authFetchById(subject1.subjectId)
                    .then(res => {
                        setCategories1(res.data);
                    })
                    .catch(err => alert(err));
            }
        }
    }, [subject1]);

    const [category1, setCategory1] = useState('');

    const handleCategorySubmit = e => {
        e.preventDefault();

        createAPIEndpoint(ENDPOINTS.categories)
            .authPost({ subjectId: subject1.subjectId, categoryName: category1 })
            .then(res => {
                alert(res.data.message);
            })
            .catch(err => alert(err));
    };

    const handleCategorySubjectChange = e => {
        const subjectId = e.target.value;
        const subjectName = e.target.options[e.target.selectedIndex].text;
        setSubject1({ subjectId: subjectId, subjectName: subjectName });
    };

    return (
        <Container className='bg-violet-300 p-10 space-y-5'>
            <Row className='pb-10'>
                <h2 className='font-bold text-center'>Categorii</h2>
            </Row>
            <Row>
                <Form onSubmit={handleCategorySubmit} className='space-y-5'>
                    <FormGroup>
                        <h4 className='font-bold'>Selectează un subiect</h4>
                        <Form.Select required onChange={handleCategorySubjectChange}>
                            {
                                subjects1.length ?
                                    subjects1.map((subject) => (
                                        <option value={subject.subjectId}>{subject.subjectName}</option>
                                    ))
                                    : <></>
                            }
                        </Form.Select>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Adaugă o categorie</h4>
                        <Stack direction="horizontal" className='space-x-5'>
                            <FormControl required type='text' onChange={(e) => setCategory1(e.target.value)} placeholder="Introdu denumirea categoriei" />
                            <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto'>Adaugă</Button>
                        </Stack>
                    </FormGroup>
                </Form>
            </Row>
            <Row>
                <h4 className='font-bold'>Vizualizează categoriile adăugate</h4>
                <div className='max-h-64 overflow-auto'>
                    <Table responsive striped bordered hover className='bg-white'>
                        <thead>
                            <tr>
                                <th>Denumire</th>
                            </tr>
                        </thead>
                        <tbody>
                            {categories1.length ? categories1.map((category) => (
                                <tr>
                                    <td>{category.categoryName}</td>
                                </tr>
                            ))
                                : <tr>
                                    <td>Nu a fost adăugată nicio categorie.</td>
                                </tr>
                            }
                        </tbody>
                    </Table>
                </div>
            </Row>
        </Container >
    )
}