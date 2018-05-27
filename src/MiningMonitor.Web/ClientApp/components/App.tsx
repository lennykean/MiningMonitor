import * as React from 'react';
import { NavLink } from 'react-router-dom';

import { Container, Row } from 'reactstrap';

import { Header, LeftNav, Routes } from '.';

export const App: React.SFC = () => (
    <>
        <LeftNav />
        <Container fluid className="right-panel">
            <Routes />
        </Container>
    </>
);
