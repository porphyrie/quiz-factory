import React, { useEffect } from 'react'
import { useState } from 'react'
import { Col, Container, Row, Stack, Button } from 'react-bootstrap';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Navigation from '../components/Navigation'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API';

export default function Test() {

    const testId = useParams().testId;

    const [testDetails, setTestDetails] = useState({});

    useEffect(() => {
        createAPIEndpoint(ENDPOINTS.tests)
            .authFetchById(testId)
            .then(res => {
                setTestDetails(res.data);
                console.log(res.data);
            })
            .catch(err => alert(err));
    }, []);

    //const [testName, setTestName] = useState('Structuri de date si algoritmi');

    // const [questions, setQuestions] = useState([
    //     "Fie vf un pointer către vârful unei stive ... .Care va fi conținutul stivei în urma execuției funcției de mai jos ?",
    //     "Se consideră următoarea listă dublu înlănțuită ... . Ce va afișa funcția ...?",
    //     "Care este sortarea topologica a grafului directionat ...?",
    //     "Care este parcurgerea DFS a grafului directionat ... incepand din nodul 0?",
    // ]);

    // //const [statistics, setStatistics] = useState({})

    // const [statistics, setStatistics] = useState({
    //     "lowestGrade": 5,
    //     "highestGrade": 9
    // });

    // //const [results, setResults] = useState([])

    // const [results, setResults] = useState([
    //     {
    //         "studentName": "Pili Denissa",
    //         "studentGrade": 7
    //     },
    //     {
    //         "studentName": "Radu Marin",
    //         "studentGrade": 9
    //     },
    //     {
    //         "studentName": "Barbu Andrei",
    //         "studentGrade": 5
    //     }
    // ]);

    const navigate = useNavigate();

    const handleModify = e => {
        navigate("/addquestions");
    };

    return (
        <Container className='bg-violet-300 p-10 space-y-5'>
            <Row className='pb-10'>
                <h2 className='font-bold text-center'>{testDetails.testName}</h2>
            </Row>
            <Row>
                <Stack direction="horizontal">
                    <div>
                        <h6 className='font-bold'>{testDetails.testDate}</h6>
                        <h6 className='font-bold'>Durata: {testDetails.testDuration} min</h6>
                        <h6 className='font-bold'>Nr. itemi: {testDetails.testItemCount}</h6>
                    </div>
                    <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto' onClick={handleModify}>Modifica</Button>
                </Stack>
            </Row>
            <Row>
                <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
                    <h4 className='font-bold flex-none'>Itemi</h4>
                    <Stack className='bg-white px-3 py-3 flex-grow'>
                        {testDetails.questionTypes.map((q, i) => (<h6>{i + 1}. {q.templateString}</h6>))}
                    </Stack>
                </Col>
                <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
                    <h4 className='font-bold'>Statistici</h4>
                    <Stack className='bg-white px-3 py-3'>
                        {
                            Object.keys(testDetails.stats.length) !== 0
                                ? <>
                                    <Stack direction="horizontal">
                                        <h6>NOTA CEA MAI MARE</h6>
                                        <h6 className='ms-auto'>{testDetails.stats.highestGrade}</h6>
                                    </Stack>
                                    <Stack direction="horizontal">
                                        <h6>NOTA CEA MAI MICA</h6>
                                        <h6 className='ms-auto'>{testDetails.stats.lowestGrade}</h6>
                                    </Stack>
                                    <Stack direction="horizontal">
                                        <h6>TIMPUL MEDIU DE RĂSPUNS</h6>
                                        <h6 className='ms-auto'>{testDetails.stats.avgResponseTime}</h6>
                                    </Stack>
                                </>
                                : <p className='text-black'>Nu este nicio statistica disponibila.</p>
                        }
                    </Stack>
                    <h4 className='font-bold'>Rezultate</h4>
                    <Stack className='bg-white px-3 py-3'>
                        {testDetails.results.length > 0
                            ? testDetails.results.map((r) => (
                                <Stack direction="horizontal">
                                    <h6><a href={'/testresults/?username=' + r.username + '&testId=' + testDetails.testId} className='text-violet-600 hover:text-violet-900'>{r.lastName} {r.firstName}</a></h6>
                                    <h6 className='ms-auto'>{r.grade}</h6>
                                </Stack>
                            ))
                            : <h6>Nu este niciun rezultat disponibil.</h6>
                        }
                    </Stack>
                </Col>
            </Row>
        </Container>
    )
}