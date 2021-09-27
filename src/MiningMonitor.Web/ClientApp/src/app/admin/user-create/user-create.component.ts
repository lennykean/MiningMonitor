import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

import { UserService } from '../user.service';

function passwordMatch(control: AbstractControl) {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');

  if (password?.value !== confirmPassword?.value) {
    return { passwordMatch: true };
  }
}

@Component({
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss'],
})
export class UserCreateComponent implements OnInit {
  validationErrors: { [key: string]: string[] } = {};
  userForm: FormGroup;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    this.userForm = this.formBuilder.group(
      {
        username: ['', Validators.required],
        password: ['', [Validators.required]],
        confirmPassword: ['', [Validators.required]],
        email: '',
      },
      {
        validators: passwordMatch,
      }
    );
  }

  isInvalid(fieldName: string) {
    const field = this.userForm.get(fieldName);
    return (
      (field.touched || field.dirty) &&
      (!field.valid || this.validationErrors[fieldName]?.length)
    );
  }

  async save() {
    try {
      await this.userService.Create(this.userForm.value);
      this.router.navigateByUrl('admin/users');
      this.validationErrors = {};
    } catch (error) {
      if (error instanceof HttpErrorResponse && error.status === 400) {
        this.validationErrors = error.error;
      }
    }
  }
}
