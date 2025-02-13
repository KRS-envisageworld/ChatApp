import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Chatservices } from "../../services/chatservices.service";
import { ChatComponent } from '../chat/chat.component';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, ChatComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  userForm: FormGroup = new FormGroup({});
  submitted = false;
  apiErrorMessages: string[] = [];
  openChat = false;
  constructor(
    private formBuilder: FormBuilder,
    private chatservice: Chatservices
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.userForm = this.formBuilder.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20),
        ],
      ],
    });
  }

  submitForm() {
    this.submitted = true;
    this.apiErrorMessages = [];
    debugger;
    if (this.userForm.valid) {
      this.chatservice.registerUser(this.userForm.value).subscribe({
        next: () => {
          debugger
          this.chatservice.myChatName = this.userForm.get('name')?.value;
          this.openChat = true;
          this.userForm.reset();
          this.submitted = false;
        },
        error: (err) => {
          if (typeof err.error === 'string') {
            this.apiErrorMessages.push(err.error);
          }
        },
      });
    } else {
      console.log('Form is invalid');
    }
  }
  closeChat() {
    this.openChat = false;
  }
}