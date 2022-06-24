import React, { useEffect, useState } from "react";
import { Button, Container, Form, FormControl, FormGroup, Row, Stack, Table, ToggleButton, ToggleButtonGroup } from "react-bootstrap";
import { createAPIEndpoint, ENDPOINTS } from "../../helpers/API";

export default function AddObjects() {

    const [isSubjectSubmitted, setIsSubjectSubmitted] = useState(0);

    const [subject, setSubject] = useState('');

    const handleSubjectSubmit = e => {
        e.preventDefault();

        createAPIEndpoint(ENDPOINTS.subjects)
            .authPost({ subjectName: subject })
            .then(res => {
                alert(res.data.message);
            })
            .catch(err => alert(err));

        setIsSubjectSubmitted(isSubjectSubmitted + 1);
    };

    const [subjects, setSubjects] = useState([]);

    useEffect(() => {
        createAPIEndpoint(ENDPOINTS.subjects)
            .authFetch()
            .then(res => {
                setSubjects(res.data);
            })
            .catch(err => alert(err));
    }, [isSubjectSubmitted]);

    ////////////

    const [subjects1, setSubjects1] = useState([]);

    const [subject1, setSubject1] = useState({});

    useEffect(() => {
        createAPIEndpoint(ENDPOINTS.subjects)
            .authFetch()
            .then(res => {
                setSubjects1(res.data);
                setSubject1(res.data[0]);
            })
            .catch(err => alert(err));
    }, []);

    const [categories1, setCategories1] = useState([])

    useEffect(() => {
        if (subject1) {
            console.log("haha");
            if (Object.keys(subject1).length) {
                createAPIEndpoint(ENDPOINTS.categories)
                    .authFetchById(subject1.subjectId)
                    .then(res => {
                        setCategories1(res.data);
                    })
                    .catch(err => alert(err));
            }
        }
    }, [subject1]);

    const [category1, setCategory1] = useState('');

    const handleCategorySubmit = e => {
        e.preventDefault();

        createAPIEndpoint(ENDPOINTS.categories)
            .authPost({ subjectId: subject1.subjectId, categoryName: category1 })
            .then(res => {
                alert(res.data.message);
            })
            .catch(err => alert(err));
    };

    const handleCategorySubjectChange = e => {
        const subjectId = e.target.value;
        const subjectName = e.target.options[e.target.selectedIndex].text;
        setSubject1({ subjectId: subjectId, subjectName: subjectName });
    };

    //////////////////

    const [subjects2, setSubjects2] = useState([]);

    useEffect(() => {
        createAPIEndpoint(ENDPOINTS.subjects)
            .authFetch()
            .then(res => {
                setSubjects2(res.data);
                setSubject2(res.data[0]);
            })
            .catch(err => alert(err));
    }, []);

    const [subject2, setSubject2] = useState({});

    const [categories2, setCategories2] = useState([])

    useEffect(() => {
        if (subject2) {
            if (Object.keys(subject2).length) {
                createAPIEndpoint(ENDPOINTS.categories)
                    .authFetchById(subject2.subjectId)
                    .then(res => {
                        setCategories2(res.data);
                        setCategory2(res.data[0]);
                    })
                    .catch(err => alert(err));
            }
        }
    }, [subject2]);

    const handleQuestionSubjectChange = e => {
        const subjectId = e.target.value;
        const subjectName = e.target.options[e.target.selectedIndex].text;
        setSubject2({ subjectId: subjectId, subjectName: subjectName });
    };

    const [category2, setCategory2] = useState({});

    const handleQuestionCategoryChange = e => {
        const categoryId = e.target.value;
        const categoryName = e.target.options[e.target.selectedIndex].text;
        setCategory2({ categoryId: categoryId, categoryName: categoryName });
    };

    const [questionTemplate, setQuestionTemplate] = useState('');

    const [configurationFile, setConfigurationFile] = useState('');

    const handleConfigurationFile = e => {
        let reader = new FileReader();
        reader.addEventListener("loadend", () => {
            setConfigurationFile(reader.result);
        });
        reader.readAsText(e.target.files[0]);
    }

    const [producingFile, setProducingFile] = useState('');

    const handleProducingFile = e => {
        let reader = new FileReader();
        reader.addEventListener("loadend", () => {
            setProducingFile(reader.result);
        });
        reader.readAsText(e.target.files[0]);
    }

    const [needsGrammar, setNeedsGrammar] = useState(false);

    const handleNeedsGrammarChange = e => {
        if (e.target.value === 'DA')
            setNeedsGrammar(false);
        else if (e.target.value === 'NU')
            setNeedsGrammar(true);
    };

    const [grammarFile, setGrammarFile] = useState('');

    const handleGrammarFile = e => {
        let reader = new FileReader();
        reader.addEventListener("loadend", () => {
            setGrammarFile(reader.result);
        });
        reader.readAsText(e.target.files[0]);
    }

    const handleQuestionSubmit = e => {
        e.preventDefault();

        let question = {
            subjectId: subject2.subjectId,
            categoryId: category2.categoryId,
            questionTemplateString: questionTemplate,
            configurationFile: configurationFile,
            producingFile: producingFile,
        };

        if (needsGrammar)
            question = { ...question, grammarFile: grammarFile };

        console.log(question);

        createAPIEndpoint(ENDPOINTS.questions)
            .authPost(question)
            .then(res => {
                alert(res.data.message);
            })
            .catch(err => alert(err));


    };

    return (
        <Container className='bg-violet-300 p-10 space-y-5'>
            <Row className='pb-10'>
                <h2 className='font-bold text-center'>Subiecte</h2>
            </Row>
            <Row>
                <Form onSubmit={handleSubjectSubmit}>
                    <h4 className='font-bold'>Adaugă un subiect</h4>
                    <Stack direction="horizontal" className='space-x-5'>
                        <FormControl required type='text' onChange={(e) => setSubject(e.target.value)} placeholder="Introdu denumirea subiectului" />
                        <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto'>Adaugă</Button>
                    </Stack>
                </Form>
            </Row>
            <Row>
                <h4 className='font-bold'>Vizualizează subiectele adăugate</h4>
                <div className='max-h-64 overflow-auto'>
                    <Table responsive striped bordered hover className='bg-white'>
                        <thead className='sticky-top'>
                            <tr>
                                <th>Denumire</th>
                            </tr>
                        </thead>
                        <tbody>
                            {subjects.length ?
                                subjects.map((subject) => (
                                    <tr>
                                        <td>{subject.subjectName}</td>
                                    </tr>
                                ))
                                : <tr>
                                    <td>Nu a fost adăugat niciun subiect.</td>
                                </tr>}
                        </tbody>
                    </Table>
                </div>
            </Row>
            <Row className='pb-10'>
                <h2 className='font-bold text-center'>Categorii</h2>
            </Row>
            <Row>
                <Form onSubmit={handleCategorySubmit} className='space-y-5'>
                    <FormGroup>
                        <h4 className='font-bold'>Selectează un subiect</h4>
                        <Form.Select required onChange={handleCategorySubjectChange}>
                            {
                                subjects1.length ?
                                    subjects1.map((subject) => (
                                        <option value={subject.subjectId}>{subject.subjectName}</option>
                                    ))
                                    : <></>
                            }
                        </Form.Select>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Adaugă o categorie</h4>
                        <Stack direction="horizontal" className='space-x-5'>
                            <FormControl required type='text' onChange={(e) => setCategory1(e.target.value)} placeholder="Introdu denumirea categoriei" />
                            <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto'>Adaugă</Button>
                        </Stack>
                    </FormGroup>
                </Form>
            </Row>
            <Row>
                <h4 className='font-bold'>Vizualizează categoriile adăugate</h4>
                <div className='max-h-64 overflow-auto'>
                    <Table responsive striped bordered hover className='bg-white'>
                        <thead>
                            <tr>
                                <th>Denumire</th>
                            </tr>
                        </thead>
                        <tbody>
                            {categories1.length ? categories1.map((category) => (
                                <tr>
                                    <td>{category.categoryName}</td>
                                </tr>
                            ))
                                : <tr>
                                    <td>Nu a fost adăugată nicio categorie.</td>
                                </tr>
                            }
                        </tbody>
                    </Table>
                </div>
            </Row>
            <Row className='pb-10'>
                <h2 className='font-bold text-center'>Întrebări</h2>
            </Row>
            <Row>
                <Form className='space-y-5' onSubmit={handleQuestionSubmit}>
                    <FormGroup>
                        <h4 className='font-bold'>Selectează un subiect</h4>
                        <Form.Select onChange={handleQuestionSubjectChange}>
                            {subjects2.map((subject) => (
                                <option value={subject.subjectId}>{subject.subjectName}</option>
                            ))}
                        </Form.Select>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Selectează o categorie</h4>
                        <Form.Select onChange={handleQuestionCategoryChange}>
                            {categories2.length ? categories2.map((category) => (
                                <option value={category.categoryId}>{category.categoryName}</option>
                            )) :
                                <option>Nu există categorii.</option>}
                        </Form.Select>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Tiparul întrebării</h4>
                        <FormControl required as="textarea" placeholder='Introdu tiparul întrebării' onChange={(e) => setQuestionTemplate(e.target.value)} />
                        <Form.Text className='text-neutral-900'>
                            Tiparul trebuie să fie asemănător cu cel din fișierul de configurare. Acest tipar va fi modul de identificare al întrebării în momentul adăugării itemilor în cadrul unui test.
                        </Form.Text>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Încarcă fișierul de configurare</h4>
                        <Form.Control required type="file" onChange={handleConfigurationFile} accept='.json' />
                        <Form.Text className='text-neutral-900'>
                            Acest fișier trebuie să conțină obligatoriu întrebarea și tiparele care marchează locurile libere ce vor fi înlocuite.
                        </Form.Text>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Încarcă fișierul producător</h4>
                        <Form.Control required type="file" onChange={handleProducingFile} accept='.cpp' />
                        <Form.Text className='text-neutral-900'>
                            Pentru a genera o varietate de întrebări pe baza tiparului furnizat anterior este necesar un fișier care compilat și rulat să printeze datele ipotezei și răspunsul corect.
                        </Form.Text>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Fișierul producător poate fi compilat în forma actuală?</h4>
                        <Form.Check type='radio' label='DA' value='DA' name='choice' onChange={handleNeedsGrammarChange} defaultChecked />
                        <Form.Check type='radio' label='NU' value='NU' name='choice' onChange={handleNeedsGrammarChange} />
                        <Form.Text className='text-neutral-900'>
                            În cazul în care fișierul producător trebuie să fie modificat pentru a putea fi compilat, va fi necesar un fișier cu gramatica care va genera porțiunea de cod customizată.
                        </Form.Text>
                    </FormGroup>
                    <FormGroup>
                        <h4 className='font-bold'>Încarcă fișierul cu gramatica</h4>
                        {needsGrammar ? <Form.Control required type="file" onChange={handleGrammarFile} accept='.json' /> : <Form.Control type="file" disabled />}
                        <Form.Text className='text-neutral-900'>
                            Acest fișier va produce funcția customizată.
                        </Form.Text>
                    </FormGroup>
                    <FormGroup>
                        <Button type='submit' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto'>Adaugă</Button>
                    </FormGroup>
                </Form>
            </Row>
        </Container >
    )
}