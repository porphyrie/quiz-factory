import moment from 'moment';
import React, { useEffect } from 'react'
import { useState } from 'react';
import { Col, Container, Form, Row, Stack, Button, FormGroup } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Navigation from '../../components/Navigation';
import { createAPIEndpoint, ENDPOINTS } from '../../helpers/API';

const ceva = {
  data: {
    columns: [],
    rows: []
  }
}


export default function AddQuestions() {

  const navigate = useNavigate();

  const getUsername = () => {
    const userData = JSON.parse(localStorage.getItem('user'));
    if (userData === null)
      return '';
    return userData.username;
  }

  const [testName, setTestName] = useState('');

  const [testDate, setTestDate] = useState();

  const [testDuration, setTestDuration] = useState();

  const [courses, setCourses] = useState([]);

  const [course, setCourse] = useState({});

  useEffect(() => {
    createAPIEndpoint(ENDPOINTS.courses)
      .authFetchById(getUsername())
      .then(res => {
        setCourses(res.data);
        setCourse({ courseId: res.data[0].courseId, courseName: res.data[0].courseName });
      })
      .catch(err => alert(err));
  }, []);

  const handleCourseChange = (e) => {
    setCourse({ courseId: e.target.value, courseName: e.target.options[e.target.selectedIndex].text });
  }

  const [subjects, setSubjects] = useState([]);

  const [subject, setSubject] = useState({});

  useEffect(() => {
    createAPIEndpoint(ENDPOINTS.subjects)
      .authFetch()
      .then(res => {
        setSubjects(res.data);
        setSubject(res.data[0]);
      })
      .catch(err => alert(err));
  }, []);

  const handleSubjectChange = (e) => {
    setSubject({ subjectId: e.target.value, subjectName: e.target.options[e.target.selectedIndex].text });
  }

  const [categories, setCategories] = useState([])

  const [category, setCategory] = useState({});

  useEffect(() => {
    if (subject.subjectId !== undefined) {
      createAPIEndpoint(ENDPOINTS.categories)
        .authFetchById(subject.subjectId)
        .then(res => {
          setCategories(res.data);
          setCategory(res.data[0]);
        })
        .catch(err => alert(err));
    }
  }, [subject]);

  const handleCategoryChange = (e) => {
    setCategory({ categoryId: e.target.value, categoryName: e.target.options[e.target.selectedIndex].text });
  }

  const [questions, setQuestions] = useState([])

  useEffect(() => {
    if (category !== undefined) {
      createAPIEndpoint(ENDPOINTS.questions)
        .authFetchByParams({ subjectId: subject.subjectId, categoryId: category.categoryId })
        .then(res => {
          setQuestions(res.data);
        })
        .catch(err => alert(err));
    }
  }, [category, subject]);

  const [addedQuestions, setAddedQuestions] = useState([]);

  const AddQuestion = (question) => {
    setAddedQuestions([...addedQuestions, question]);
  }

  const RemoveQuestion = (i) => {
    setAddedQuestions(addedQuestions.filter((question, index) => index !== i));
  }

  const handleSubmit = e => {
    e.preventDefault();

    const questionIds = addedQuestions.map(question => question.questionId);

    const test = {
      courseId: course.courseId,
      testName: testName,
      testDate: testDate,
      testDuration: testDuration,
      questions: questionIds
    }

    console.log(test);

    createAPIEndpoint(ENDPOINTS.tests)
      .authPost(test)
      .then(res => {
        alert(res.data.message);
        navigate("/tests");
      })
      .catch(err => alert(err));

  }

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      <Form onSubmit={handleSubmit}>
        <Row>
          <Col className='bg-violet-300 py-3 space-y-5 flex flex-col'>
            <FormGroup>
              <Form.Label className='font-bold text-xl mb-3'>Denumirea testului</Form.Label>
              <Form.Control required type='text' placeholder='Introdu denumirea testului' onChange={(e) => setTestName(e.target.value)} />
            </FormGroup>
            <FormGroup>
              <Form.Label className='font-bold text-xl mb-3'>Data și ora începerii testului</Form.Label>
              <Form.Control required type="datetime-local" max={moment().add(1, 'M').format('YYYY-MM-DDTHH:mm')} onChange={(e) => setTestDate(e.target.value)} />
            </FormGroup>
            <FormGroup>
              <Form.Label className='font-bold text-xl mb-3'>Durata testului</Form.Label>
              <Form.Control required type='number' min='5' max='180' placeholder="Introdu numarul de minute" onChange={(e) => setTestDuration(e.target.value)} />
            </FormGroup>
            <FormGroup>
              <Form.Label className='font-bold text-xl mb-3'>Selectează cursul către care va fi livrat</Form.Label>
              <Form.Select onChange={handleCourseChange}>
                {courses.map((course) => (
                  <option value={course.courseId}>{course.courseName}</option>
                ))}
              </Form.Select>
            </FormGroup>
          </Col>
        </Row>
        <Row>
          <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
            <FormGroup>
              <Form.Label className='font-bold text-xl mb-3'>Selectează subiectul</Form.Label>
              <Form.Select onChange={handleSubjectChange}>
                {subjects.length ? subjects.map((subject) => (
                  <option value={subject.subjectId}>{subject.subjectName}</option>
                )) : <option>Nu a fost găsit niciun subiect</option>}
              </Form.Select>
            </FormGroup>
            <FormGroup>
              <Form.Label className='font-bold text-xl mb-3'>Selectează categoria</Form.Label>
              <Form.Select onChange={handleCategoryChange} >
                {categories.length ? categories.map((category) => (
                  <option value={category.categoryId}>{category.categoryName}</option>
                )) : <option>Nu a fost găsită nicio categorie</option>}
              </Form.Select>
            </FormGroup>
            <FormGroup>
              <Form.Label className='font-bold text-xl mb-3'>Adaugă întrebarile</Form.Label>
              <Stack className='bg-white px-3 py-3 space-y-3'>
                {questions.length ? questions.map((question, i) => (
                  <div className='flex space-x-3 items-center'>
                    <h6 className="grow">{i + 1}. {question.questionTemplateString}</h6>
                    <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white flex-none' onClick={() => AddQuestion(question)}>+</Button>
                  </div>
                )) : <h6>Nu a fost găsită nicio întrebare</h6>}
              </Stack>
            </FormGroup>
          </Col>
          <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
            <FormGroup>
              <Stack direction="horizontal">
                <Form.Label className='font-bold text-xl mb-3'>Previzualizează testul</Form.Label>
                <Form.Label className='font-bold text-xl ms-auto mb-3'>{addedQuestions.length} itemi</Form.Label>
              </Stack>
              <Stack className='bg-white px-3 py-3 space-y-3 min-h-64'>
                {addedQuestions.length
                  ? addedQuestions.map((question, i) => (
                    <div className='flex space-x-3 items-center'>
                      <h6 className="grow">{i + 1}. {question.questionTemplateString}</h6>
                      <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white flex-none' onClick={() => RemoveQuestion(i)}>-</Button>
                    </div>))
                  : <h6 className='px-2 py-1'>Nu a fost adaugată nicio intrebare.</h6>}
              </Stack>
            </FormGroup>
            <FormGroup>
              <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white'>Finalizează</Button>
            </FormGroup>
          </Col>
        </Row >
      </Form>
    </Container >
  )
}