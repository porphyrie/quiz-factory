import React from 'react'
import { useState } from 'react'
import { Container, FormGroup, Row, Button, Form, Col } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Navigation from '../../components/Navigation'

export default function SetTestDetails() {

  const [data, setData] = useState(
    [
      "Grupa C114A",
      "Grupa C114B",
      "Grupa C114C",
      "Restanțieri la SDA"
    ]
  )

  const navigate = useNavigate();

  const handleFinish = e => {
    navigate("/tests");
  }

  const handleSave = e => {
    navigate("/tests");
  }

  const [modify, setModify] = useState(0);

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      <Row>
        <Col className='bg-violet-300 py-3 space-y-5 flex flex-col'>
          <FormGroup>
            <Form.Label className='font-bold text-xl mb-3'>Denumirea testului</Form.Label>
            <Form.Control type='text' placeholder='Introdu denumirea testului' />
          </FormGroup>
          <FormGroup>
            <Form.Label className='font-bold text-xl mb-3'>Data și ora începerii testului</Form.Label>
            <Form.Control type="datetime-local" />
          </FormGroup>
          <FormGroup>
            <Form.Label className='font-bold text-xl mb-3'>Durata testului</Form.Label>
            <Form.Control type='number' min='5' max='180' placeholder="Introdu numarul de minute" />
          </FormGroup>
          <FormGroup>
            <Form.Label className='font-bold text-xl mb-3'>Selectează cursul către care va fi livrat</Form.Label>
            <Form.Select>
              {data.map((data) => (
                <option>{data}</option>
              ))}
            </Form.Select>
          </FormGroup>
          <FormGroup>
            {
              modify === 0
                ? <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleFinish}>Finalizează</Button>
                : <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleSave}>Salvează</Button>
            }
          </FormGroup>
        </Col>
      </Row>
    </Container>
  )
}
