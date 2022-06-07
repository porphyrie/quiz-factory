import React, { useState } from 'react'
import { Button, Container, Nav, Navbar, NavDropdown, Row } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

export default function Navigation() {

    const getUsername = () => {
        const userData = JSON.parse(localStorage.getItem('user'));
        if (userData === null)
            return '';
        return userData.username;
    }

    const getUserType = () => {
        const userData = JSON.parse(localStorage.getItem('user'));
        if (userData === null)
            return '';
        return userData.role;
    }

    const navigate = useNavigate();

    const handleLogout = e => {
        localStorage.clear();
        navigate("/");
    };

    const handleLogin = e => {
        navigate("/login");
    };

    return (
        <Container>
            <Row>
                <Navbar expand="lg" variant="light" bg="light">
                    <Container>
                        <Navbar.Brand href='/tests' className='text-violet-900 font-black text-4xl'>EDUGEN</Navbar.Brand>
                        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
                        {getUsername() !== ''
                            ? <Navbar.Collapse id="responsive-navbar-nav">
                                <Nav className="me-auto">
                                    <Nav.Link href="#">Link</Nav.Link>
                                    <NavDropdown title="Activități" id="basic-nav-dropdown">
                                        <NavDropdown.Item href="/tests">Teste</NavDropdown.Item>
                                        <NavDropdown.Item href="/courses">Cursuri</NavDropdown.Item>
                                    </NavDropdown>
                                </Nav>
                                <Nav>
                                    <Nav.Link disabled className='text-neutral-500'>@{getUsername()}</Nav.Link>
                                    <Button type='button' variant="default" className='bg-violet-900 hover:bg-violet-600 text-white' onClick={handleLogout}>Deloghează-te</Button>
                                </Nav>
                            </Navbar.Collapse>
                            : <Navbar.Collapse className="justify-content-end">
                                <Button type='button' variant="default" className='bg-violet-900 hover:bg-violet-600 text-white' onClick={handleLogin}>Loghează-te</Button>
                            </Navbar.Collapse>
                        }
                    </Container>
                </Navbar>
            </Row>
        </Container>
    )
}