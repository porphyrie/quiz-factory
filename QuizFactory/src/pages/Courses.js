import { Formik } from 'formik';
import * as yup from 'yup';
import React, { useEffect, useState } from 'react'
import { Button, Container, Form, FormGroup, Row, Stack, Table } from 'react-bootstrap'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API';

export default function Courses() {

    const [courses, setCourses] = useState([])

    const getUserType = () => {
        const userData = JSON.parse(localStorage.getItem('user'));
        if (userData === null)
            return '';
        return userData.role;
    }

    const getUsername = () => {
        const userData = JSON.parse(localStorage.getItem('user'));
        if (userData === null)
            return '';
        return userData.username;
    }

    const schema = yup.object().shape({
        coursename: yup.string().required('Required'),
    });

    const initVal = {
        coursename: ''
    }

    const [isSubmitted, setIsSubmitted] = useState(0);

    const [chosenCourse, setChosenCourse] = useState({});

    const handleSubmit = (values) => {
        if (getUserType() === 'profesor') {
            const fields = {
                coursename: values.coursename,
                professorusername: getUsername()
            }

            createAPIEndpoint(ENDPOINTS.courses)
                .authPost(fields)
                .then(res => {
                    alert(res.data.message);
                })
                .catch(err => alert(err));

            setIsSubmitted(isSubmitted + 1);
            console.log(isSubmitted);
        }
        else if (getUserType() === 'student') {
            const fields = {
                courseId: chosenCourse.courseId,
                studentUsername: getUsername()
            }
            createAPIEndpoint(ENDPOINTS.enrollment)
                .authPost(fields)
                .then(res => {
                    alert(res.data.message);
                })
                .catch(err => alert(err));

            setIsSubmitted(isSubmitted + 1);
            console.log(isSubmitted);
        }
    }

    const [enrolledCourses, setEnrolledCourses] = useState([]);

    useEffect(() => {
        console.log(getUserType());
        if (getUserType() === 'profesor') {
            console.log('a trimis');
            createAPIEndpoint(ENDPOINTS.courses)
                .authFetchById(getUsername())
                .then(res => {
                    console.log(res.data);
                    setCourses(res.data);
                })
                .catch(err => alert(err));
        }
        else if (getUserType() === 'student') {
            createAPIEndpoint(ENDPOINTS.courses)
                .authFetchById(getUsername())
                .then(res => {
                    setEnrolledCourses(res.data);
                    console.log(res.data);
                })
                .catch(err => alert(err));

            createAPIEndpoint(ENDPOINTS.courses)
                .authFetch()
                .then(res => {
                    setCourses(res.data);
                    if (!isSubmitted)
                        setChosenCourse(res.data[0]);
                })
                .catch(err => alert(err));
        }
    }, [isSubmitted]);

    const handleCourseChange = e => {
        const courseId = e.target.value;
        const courseName = e.target.options[e.target.selectedIndex].text;
        setChosenCourse({ courseId: courseId, courseName: courseName });
    };

    return (
        <Container className='bg-violet-300 p-10 space-y-5'>
            <Row className='pb-10'>
                <h2 className='font-bold text-center'>Cursuri</h2>
            </Row>
            <Row>
                {getUserType() === 'profesor'
                    ?
                    <>
                        <h4 className='font-bold'>Creează un curs</h4>
                        <Formik
                            validationSchema={schema}
                            onSubmit={handleSubmit}
                            initialValues={initVal}
                        >
                            {({
                                handleSubmit,
                                handleChange,
                                handleBlur,
                                values,
                                touched,
                                errors,
                                isSubmitting
                            }) => (
                                <Form noValidate>
                                    <Stack direction="horizontal" className='space-x-5 items-start'>
                                        <FormGroup>
                                            <Form.Control type='text' name="coursename" value={values.coursename} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.coursename && errors.coursename} isValid={touched.coursename && !errors.coursename} placeholder="Introdu denumirea cursului" />
                                            <Form.Control.Feedback type="invalid">
                                                {errors.coursename}
                                            </Form.Control.Feedback>
                                        </FormGroup>
                                        <FormGroup>
                                            <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto' onClick={handleSubmit}>Creează</Button>
                                        </FormGroup>
                                    </Stack>
                                </Form>
                            )}
                        </Formik >
                    </>
                    :
                    <>
                        <h4 className='font-bold'>Înscrie-te într-un curs</h4>
                        <Stack direction="horizontal" className='space-x-5'>
                            <Form.Select required onChange={handleCourseChange}>
                                {courses.map((course) => (
                                    <option value={course.courseId}>{course.courseName}</option>
                                ))}
                            </Form.Select>
                            <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto w-fit text-nowrap' onClick={handleSubmit}>Înscrie-te</Button>
                        </Stack>
                    </>
                }
            </Row>
            <Row>
                {getUserType() === 'profesor'
                    ?
                    <>
                        <h4 className='font-bold'>Vizualizează cursurile create de tine</h4>
                        <Table responsive striped bordered hover className='bg-white'>
                            <thead>
                                <tr>
                                    <th>Denumire</th>
                                    <th>Nr. participanți</th>
                                    <th>Participanți</th>
                                </tr>
                            </thead>
                            <tbody>
                                {courses.map((course) => (
                                    <tr>
                                        <td>{course.courseName}</td>
                                        <td>{course.participantsCount}</td>
                                        <td>
                                            <Stack className='max-h-64 overflow-auto'>
                                                {course.participantsCount
                                                    ? course.participants.map((participant) => (
                                                        <h6>{participant.username}</h6>
                                                    ))
                                                    : <h6>Nu sunt studenți înrolați.</h6>
                                                }
                                            </Stack>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                    </>
                    : <>
                        <h4 className='font-bold'>Vizualizează cursurile în care te-ai înscris</h4>
                        <Table responsive striped bordered hover className='bg-white'>
                            <thead>
                                <tr>
                                    <th>Denumire</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    enrolledCourses.length
                                        ? enrolledCourses.map((course) => (
                                            <tr>
                                                <td>{course.courseName}</td>
                                            </tr>
                                        ))
                                        : <tr><td>Nu ești înscris în niciun curs.</td></tr>
                                }
                            </tbody>
                        </Table>
                    </>
                }
            </Row>
        </Container >
    )

}