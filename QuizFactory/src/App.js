import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import CreateTest from './pages/teacher/CreateTest';
import Tests from './pages/Tests';
import TestDetails from './pages/TestDetails';
import Question from './pages/student/Question';
import TestResults from './pages/TestResults';
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Navigation from './components/Navigation';
import { Container, Row } from 'react-bootstrap';
import Register from './pages/Register';
import Courses from './pages/Courses';
import AddSubjects from './pages/admin/AddSubjects';
import AddCategories from './pages/admin/AddCategories';
import AddQuestions from './pages/admin/AddQuestions';
import { getUserType } from './helpers/User';

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
              <Route path="/" element={getUserType() === '' ? <Login /> : getUserType() !== 'admin' ? <Tests /> : <AddQuestions />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/createtest" element={<CreateTest />} />
              <Route path="/tests" element={<Tests />} />
              <Route path="/testdetails/:testId" element={<TestDetails />} />
              <Route path="/question" element={<Question />} />
              <Route path="/testresults" element={<TestResults />} />
              <Route path="/courses" element={<Courses />} />
              <Route path="/addsubjects" element={<AddSubjects />} />
              <Route path="/addcategories" element={<AddCategories />} />
              <Route path="/addquestions" element={<AddQuestions />} />
            </Routes>
          </Row>
        </Container>
      </div>
    </BrowserRouter >
  );
}
