import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import AddQuestions from './pages/teacher/AddQuestions';
import Tests from './pages/Tests';
import Test from './pages/Test';
import Question from './pages/student/Question';
import TestResults from './pages/TestResults';
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Navigation from './components/Navigation';
import { Container, Row } from 'react-bootstrap';
import Register from './pages/Register';
import Courses from './pages/Courses';
import AddObjects from './pages/admin/AddObjects';

export default function App() {

  return (
    <BrowserRouter>
      <div className='bg-violet-300'>
        <Container className='h-screen bg-violet-200 flex flex-col overflow-auto'>
          <Row className='flex-none sticky-top border-bottom'>
            <Navigation></Navigation>
          </Row>
          <Row className='flex-grow items-center px-4'>
            <Routes>
              <Route path="/" element={<Login />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/addquestions" element={<AddQuestions />} />
              <Route path="/tests" element={<Tests />} />
              <Route path="/test" element={<Test />} />
              <Route path="/question" element={<Question />} />
              <Route path="/testresults" element={<TestResults />} />
              <Route path="/courses" element={<Courses />} />
              <Route path="/addobjects" element={<AddObjects />} />
            </Routes>
          </Row>
        </Container>
      </div>
    </BrowserRouter >
  );
}
