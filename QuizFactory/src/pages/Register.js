import { Formik } from 'formik';
import * as yup from 'yup';
import React, { useState } from 'react'
import { Col, Form, FormGroup, Row, Button, Container, InputGroup } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API';
import image from '../undraw_working_remotely_re_6b3a (1).svg'

export default function Register() {

    const navigate = useNavigate();

    const schema = yup.object().shape({
        lastname: yup.string().required('Required'),
        firstname: yup.string().required('Required'),
        username: yup.string().required('Required'),
        password: yup.string().required('Required'),
        confirmpassword: yup.string().required('Required').oneOf([yup.ref('password')], 'Passwords must match'),
        role: yup.string().required('Required')
    });

    const initVal = {
        lastname: '',
        firstname: '',
        username: '',
        password: '',
        confirmpassword: '',
        role: 'student'
    }

    const handleSubmit = (values) => {
        const { confirmpassword, ...fields } = values;
        console.log(fields);
        createAPIEndpoint(ENDPOINTS.register)
            .post(fields)
            .then(res => {
                alert('Contul tău a fost creat cu succes!')
                navigate('/login');
            })
            .catch(err => alert(err));
    }

    return (
        <Container className='bg-violet-300'>
            <Row>
                <Col sm={6} className='bg-violet-500 p-10 flex items-center'>
                    <img src={image} alt="" />
                </Col>
                <Col sm={6} className='bg-violet-300 p-10 flex items-center'>
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
                                <h5 className='mb-4'>Introdu datele cu care te vei autentifica pentru a accesa platforma.</h5>
                                <div className='space-y-4'>
                                    <FormGroup>
                                        <Form.Label className='font-bold text-base mb-3'>Nume:</Form.Label>
                                        <Form.Control type='text' name="lastname" value={values.lastname} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.lastname && errors.lastname} isValid={touched.lastname && !errors.lastname} placeholder="Introdu numele" />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.lastname}
                                        </Form.Control.Feedback>
                                    </FormGroup>
                                    <FormGroup>
                                        <Form.Label className='font-bold text-base mb-3'>Prenume:</Form.Label>
                                        <Form.Control type='text' name="firstname" value={values.firstname} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.firstname && errors.firstname} isValid={touched.firstname && !errors.firstname} placeholder="Introdu prenumele" />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.firstname}
                                        </Form.Control.Feedback>
                                    </FormGroup>
                                    <FormGroup>
                                        <Form.Label className='font-bold text-base mb-3'>Nume de utilizator:</Form.Label>
                                        <InputGroup hasValidation>
                                            <InputGroup.Text>@</InputGroup.Text>
                                            <Form.Control type='text' name="username" value={values.username} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.username && errors.username} isValid={touched.username && !errors.username} placeholder="Introdu numele de utilizator" />
                                        </InputGroup>
                                        <Form.Control.Feedback type="invalid">
                                            {errors.username}
                                        </Form.Control.Feedback>
                                    </FormGroup>
                                    <FormGroup>
                                        <Form.Label className='font-bold text-base mb-3'>Parolă:</Form.Label>
                                        <Form.Control type='password' name="password" value={values.password} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.password && errors.password} isValid={touched.password && !errors.password} placeholder="Introdu parola" />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.password}
                                        </Form.Control.Feedback>
                                    </FormGroup>
                                    <FormGroup>
                                        <Form.Label className='font-bold text-base mb-3'>Confirmă parola:</Form.Label>
                                        <Form.Control type='password' name="confirmpassword" value={values.confirmpassword} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.confirmpassword && errors.confirmpassword} isValid={touched.confirmpassword && !errors.confirmpassword} placeholder="Introdu din nou parola" />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.confirmpassword}
                                        </Form.Control.Feedback>
                                    </FormGroup>
                                    <FormGroup>
                                        <Form.Label className='font-bold text-base mb-3'>Selectează rolul sub care vei utiliza platforma:</Form.Label>
                                        <Form.Select name="role" value={values.role} onChange={handleChange}>
                                            <option>student</option>
                                            <option>profesor</option>
                                        </Form.Select>
                                        <Form.Control.Feedback type="invalid">
                                            {errors.role}
                                        </Form.Control.Feedback>
                                    </FormGroup>
                                    <FormGroup>
                                        <Button type='submit' variant="default" onClick={handleSubmit} className='bg-violet-900 hover:bg-violet-600 text-white'>Submite</Button>
                                    </FormGroup>
                                    <FormGroup>
                                        <Form.Text className='text-neutral-900'>
                                            Ai un cont deja? <a href='/login' className='text-violet-600 hover:text-violet-900'>Loghează-te!</a>
                                        </Form.Text>
                                    </FormGroup>
                                </div>
                            </Form>
                        )}
                    </Formik >
                </Col >
            </Row >
        </Container >
    );
}







