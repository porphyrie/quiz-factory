import { Formik } from 'formik';
import * as yup from 'yup';
import React, { useState } from 'react'
import { Col, Form, FormGroup, Row, Button, Container } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API';
import image from '../undraw_working_remotely_re_6b3a (1).svg'

export default function Login() {

  const navigate = useNavigate();

  const schema = yup.object().shape({
    username: yup.string().required('Required'),
    password: yup.string().required('Required'),
  });

  const initVal = {
    username: '',
    password: ''
  }

  const handleSubmit = (values) => {
    createAPIEndpoint(ENDPOINTS.login)
      .post(values)
      .then(res => {
        localStorage.setItem('user', JSON.stringify(res.data));
        if (res.data.role === "profesor" || res.data.role === "student")
          navigate('/tests');
        else
          navigate('/addquestions');
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
                <Container className='space-y-4 px-0'>
                  <h5 className='mb-4'>Introdu datele de autentificare pentru a accesa platforma.</h5>
                  <FormGroup>
                    <Form.Label className='font-bold mb-3'>Nume de utilizator:</Form.Label>
                    <Form.Control type='text' name="username" value={values.username} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.username && errors.username} isValid={touched.username && !errors.username} placeholder="Introdu numele de utilizator" />
                    <Form.Control.Feedback type="invalid">
                      {errors.username}
                    </Form.Control.Feedback>
                  </FormGroup>
                  <FormGroup>
                    <Form.Label className='font-bold mb-3'>Parolă:</Form.Label>
                    <Form.Control type='password' name="password" value={values.password} onChange={handleChange} onBlur={handleBlur} isInvalid={touched.password && errors.password} isValid={touched.password && !errors.password} placeholder="Introdu parola" />
                    <Form.Control.Feedback type="invalid">
                      {errors.password}
                    </Form.Control.Feedback>
                  </FormGroup>
                  <FormGroup>
                    <Button type='submit' onClick={handleSubmit} variant="default" className='bg-violet-900 hover:bg-violet-600 text-white'>Submite</Button>
                  </FormGroup>
                  <FormGroup>
                    <Form.Text className='text-neutral-900'>
                      Nu ai un cont? <a href='/register' className='text-violet-600 hover:text-violet-900'>Înregistrează-te!</a>
                    </Form.Text>
                  </FormGroup>
                </Container>
              </Form>
            )}
          </Formik >
        </Col>
      </Row>
    </Container >
  )
}