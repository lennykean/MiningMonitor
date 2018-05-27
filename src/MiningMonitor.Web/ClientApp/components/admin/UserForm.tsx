import * as React from 'react';

import { Button, Col, Form, FormGroup, FormText, Label } from 'reactstrap';

import autobind from 'autobind-decorator';

import { User } from '../../models';
import { FormInput } from '../common';

interface Props {
    user?: User;
    validation?: { [key: string]: string[] };
    onSubmit?: (user: User) => void;
}

interface State {
    username: string;
    email: string;
    password: string;
}

export class UserForm extends React.Component<Props, State> {
    public constructor(props: Props) {
        super(props);
        this.state = this.createState(props.user);
    }
    public componentWillReceiveProps(props: Props) {
        if (props.user === this.props.user) {
            return;
        }
        this.setState(this.createState(props.user));
    }
    @autobind
    public change(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.value,
        });
    }
    @autobind
    public submit(event: React.ChangeEvent<HTMLFormElement>) {
        event.preventDefault();
        if (this.props.onSubmit) {
            this.props.onSubmit({
                ...this.props.user,
                username: this.state.username,
                email: this.state.email,
                password: this.state.password,
            });
        }
    }
    public render() {
        return (
            <>
                <Form onSubmit={this.submit}>
                    <FormGroup row>
                        <Label for="username" sm={2}>
                            Username
                        </Label>
                        <Col sm={10}>
                            <FormInput
                                id="username"
                                name="username"
                                type="text"
                                value={this.state.username || ''}
                                onChange={this.change}
                                validation={this.props.validation}
                            />
                        </Col>
                    </FormGroup>
                    <FormGroup row>
                        <Label for="email" sm={2}>E-Mail</Label>
                        <Col sm={10}>
                            <FormInput
                                id="email"
                                name="email"
                                type="text"
                                value={this.state.email || ''}
                                onChange={this.change}
                                validation={this.props.validation}
                            />
                        </Col>
                    </FormGroup>
                    <FormGroup row>
                        <Label for="password" sm={2}>Password</Label>
                        <Col sm={10}>
                            <FormInput
                                id="password"
                                name="password"
                                type="password"
                                value={this.state.password || ''}
                                onChange={this.change}
                                validation={this.props.validation}
                            />
                        </Col>
                    </FormGroup>
                    <Button outline color="info">Save Changes</Button>
                </Form>
            </>
        );
    }
    private createState(user: User) {
        user = user || {} as User;
        return {
            username: user && user.username,
            email: user && user.email,
            password: '',
        };
    }
}
