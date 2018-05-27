import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { bindActionCreators, Dispatch } from 'redux';

import * as Icon from 'react-feather';
import { Button, Container, Form, FormGroup, Input, Label } from 'reactstrap';

import autobind from 'autobind-decorator';

import { LoginAction, loginActions } from '../actions';
import { AppState } from '../store';

interface Props {
    login: LoginAction;
}

interface State {
    username: string;
    password: string;
}

export class LoginPage extends React.Component<Props, State> {
    public constructor(props: Props) {
        super(props);
        this.state = {
            username: '',
            password: '',
        };
    }
    @autobind
    public change(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.value,
        });
    }
    @autobind
    public submit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        this.props.login(this.state.username, this.state.password);
    }
    public render() {
        return (
            <>
                <Container>
                    <Form onSubmit={this.submit}>
                        <h1>Login</h1>
                        <FormGroup className="form-group">
                            <Label for="username">Username</Label>
                            <Input type="text" name="username" value={this.state.username} onChange={this.change} />
                        </FormGroup>
                        <FormGroup>
                            <Label for="password">Password</Label>
                            <Input type="password" name="password" value={this.state.password} onChange={this.change} />
                        </FormGroup>
                        <Button outline color="info" type="submit">
                            <Icon.LogIn size="14" />&nbsp;
                            Login
                        </Button>
                    </Form>
                </Container>
            </>
        );
    }
}

const mapStateToProps = (state: AppState, ownProps) => ({
});

const mapDispatchToProps = (dispatch: Dispatch<AppState>) => ({
    login: bindActionCreators(loginActions.login, dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(LoginPage);
