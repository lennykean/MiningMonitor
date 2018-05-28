import * as React from 'react';

import { Container } from 'reactstrap';

import { LeftNav, Routes } from '.';

export const App: React.SFC = () => (
    <>
        <LeftNav />
        <Container fluid className="right-panel">
            <Routes />
        </Container>
    </>
);
