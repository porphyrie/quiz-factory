import React, { useEffect } from 'react'
import { useState } from 'react'
import { Col, Container, Row, Stack, Button } from 'react-bootstrap';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Navigation from '../components/Navigation'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API';
import { formatDate } from '../helpers/Date';

export default function TestDetails() {

    const testId = useParams().testId;

    const [testDetails, setTestDetails] = useState({});

    useEffect(() => {
        createAPIEndpoint(ENDPOINTS.test)
            .authFetchById(testId)
            .then(res => {
                setTestDetails(res.data);
                console.log(res.data);
            })
            .catch(err => alert(err));
    }, []);

    return (
        <Container className='bg-violet-300 p-10 space-y-5'>
            <Row>
                <h2 className='font-bold text-center'>{testDetails.testName}</h2>
            </Row>
            <Row>
                <h6 className='font-bold'>{formatDate(testDetails.testDate)}</h6>
                <h6 className='font-bold'>Durata: {testDetails.testDuration} min</h6>
                <h6 className='font-bold'>Nr. itemi: {testDetails.testItemCount}</h6>
            </Row>
            <Row>
                <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
                    <h4 className='font-bold flex-none'>Itemi</h4>
                    <Stack className='bg-white px-3 py-3 flex-grow'>
                        {Object.keys(testDetails).length ?
                            testDetails.questionTypes.map((q, i) => (<h6>{i + 1}. {q.templateString}</h6>))
                            : <></>}
                    </Stack>
                </Col>
                <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
                    <h4 className='font-bold'>Statistici</h4>
                    <Stack className='bg-white px-3 py-3'>
                        {
                            Object.keys(testDetails).length
                                ? testDetails.results.length ? <>
                                    <Stack direction="horizontal">
                                        <h6>PUNCTAJUL CEL MAI MARE</h6>
                                        <h6 className='ms-auto'>{testDetails.stats.highestGrade}</h6>
                                    </Stack>
                                    <Stack direction="horizontal">
                                        <h6>PUNCTAJUL CEL MAI MIC</h6>
                                        <h6 className='ms-auto'>{testDetails.stats.lowestGrade}</h6>
                                    </Stack>
                                    <Stack direction="horizontal">
                                        <h6>TIMPUL MEDIU DE RĂSPUNS</h6>
                                        <h6 className='ms-auto'>{Math.round(testDetails.stats.avgResponseTime)} min</h6>
                                    </Stack>
                                    <Stack direction="horizontal">
                                        <h6>ÎNTREBĂRILE CU CELE MAI PUȚINE RĂSPUNSURI CORECTE</h6>
                                        <h6 className='ms-auto'>{testDetails.stats.leastAnsweredQuestions.map(laq =>
                                            testDetails.questionTypes.findIndex(q => q.id === laq.id) + 1) + ' '}</h6>
                                    </Stack>
                                    <Stack direction="horizontal">
                                        <h6>ÎNTREBĂRILE CU CELE MAI MULTE RĂSPUNSURI CORECTE</h6>
                                        <h6 className='ms-auto'>{testDetails.stats.mostAnsweredQuestions.length ? testDetails.stats.mostAnsweredQuestions.map(laq =>
                                            testDetails.questionTypes.findIndex(q => q.id === laq.id) + 1) + ' ' : '-'}</h6>
                                    </Stack>
                                </>
                                    : <p className='text-black'>Nu este nicio statistica disponibila.</p>
                                : <></>
                        }
                    </Stack>
                    <h4 className='font-bold'>Rezultate</h4>
                    <Stack className='bg-white px-3 py-3'>
                        {Object.keys(testDetails).length
                            ? testDetails.results.length
                                ? testDetails.results.map((r) => (
                                    <Stack direction="horizontal">
                                        <h6><a href={'/testresults/?username=' + r.username + '&testId=' + testDetails.testId} className='text-violet-600 hover:text-violet-900'>{r.lastName} {r.firstName}</a></h6>
                                        <h6 className='ms-auto'>{r.grade}</h6>
                                    </Stack>
                                ))
                                : <h6>Nu este niciun rezultat disponibil.</h6>
                            : <></>
                        }
                    </Stack>
                </Col>
            </Row>
        </Container>
    )
}