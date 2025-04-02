import { Injectable } from '@angular/core';
import { User } from './user/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUser: User = new User();

  getUser(): User {
    return this.currentUser;
  }

  setUser(user: User): void {
    this.currentUser = user;
  }
}
