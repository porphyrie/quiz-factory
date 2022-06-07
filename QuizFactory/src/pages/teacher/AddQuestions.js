import React from 'react'
import { useState } from 'react';
import { Col, Container, Form, Row, Stack, Button, FormGroup } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Navigation from '../../components/Navigation';


export default function AddQuestions() {

  const [questions, setQuestions] = useState([
    {
      id: 0,
      subjectName: "Structuri de date si Algoritmi",
      questionCategories: [
        {
          category: "Liste",
          questionTypes: [
            "Fie vf un pointer către vârful unei stive ... . Care va fi valoarea variabilei s în urma execuției secvenței ...?",
            "Fie prim un pointer către primul nod al următoarei liste ... . Care va fi conținutul listei prin parcurgerea acesteia de la primul nod prim spre capătul listei, în urma apelului funcției ...?",
            "Se consideră următoarea listă dublu înlănțuită ... . Ce va afișa funcția ...?"

          ]
        },
        {
          category: "Grafuri",
          questionTypes: [
            "Care este sortarea topologica a grafului directionat ...?",
            "Care este parcurgerea DFS a grafului directionat ... incepand din nodul 0?"
          ]
        }
      ]
    }
  ])

  const [change, setChange] = useState(questions[0].subjectName);

  const [categorie, setCategorie] = useState(questions[0].questionCategories[0].category);

  const [addedquestion, setAddedQuestions] = useState([]);

  const AddItem = (q) => {
    console.log(q);
    setAddedQuestions([...addedquestion, {
      id: Math.floor(Math.random() * 100 - 1),
      intrebare: q,
    }]);
    console.log(addedquestion);
  }

  const RemoveItem = (q) => {
    setAddedQuestions(addedquestion.filter(element => element.id !== q))
  }

  const navigate = useNavigate();

  const handleNextStep = e => {
    navigate("/settestdetails");
  }

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      <Row>
        <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
          <FormGroup>
            <Form.Label className='font-bold text-xl mb-3'>Selectează subiectul</Form.Label>
            <Form.Select onChange={(e) => setChange(e.target.value)}>
              {questions.map((question) => (
                <option id={question.id}>{question.subjectName}</option>
              ))}
            </Form.Select>
          </FormGroup>
          <FormGroup>
            <Form.Label className='font-bold text-xl mb-3'>Selectează categoria</Form.Label>
            <Form.Select onChange={(e) => setCategorie(e.target.value)} >
              {(questions.find(subject => subject.subjectName === change)).questionCategories.map((cat) => (
                <option id={cat.category}>{cat.category}</option>
              ))}
            </Form.Select>
          </FormGroup>
          <FormGroup>
            <Form.Label className='font-bold text-xl mb-3'>Adaugă întrebarile</Form.Label>
            <Stack className='bg-white px-3 py-3'>
              {(questions.find(subject => subject.subjectName === change)).questionCategories.find((cat) => (cat.category === categorie)).questionTypes.map((q, i) => (
                <Stack direction="horizontal" className='space-x-3'>
                  <h6>{i + 1}. {q}</h6>
                  <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto' onClick={() => AddItem(q)}>+</Button>
                </Stack>
              ))}
            </Stack>
          </FormGroup>
        </Col>
        <Col sm={6} className='bg-violet-300 py-3 space-y-5 flex flex-col'>
          <FormGroup>
            <Stack direction="horizontal">
              <Form.Label className='font-bold text-xl mb-3'>Previzualizează testul</Form.Label>
              <Form.Label className='font-bold text-xl ms-auto mb-3'>{addedquestion.length} itemi</Form.Label>
            </Stack>
            <Stack className='bg-white px-3 py-3'>
              {addedquestion.length > 0
                ? addedquestion.map((quest, i) => (
                  <Stack direction="horizontal" className='space-x-3'>
                    <h6>{i + 1}. {quest.intrebare}</h6>
                    <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={() => RemoveItem(quest.id)}>-</Button>
                  </Stack>))
                : <h6 className='px-2 py-1'>Nu a fost adaugată nicio intrebare.</h6>}
            </Stack>
          </FormGroup>
          <FormGroup>
            <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleNextStep}>Pasul următor</Button>
          </FormGroup>
        </Col>
      </Row >
    </Container >
  )
}