﻿namespace TestContainerWebApi.Models.ModelDto
{
    public class UserDto
    {
        private readonly int _id;
        private DateTimeOffset _createdAt;
        private DateTimeOffset _updatedAt;

        public UserDto(int id, DateTimeOffset createdAt, DateTimeOffset updatedAt)
        {
            _id = id;
            _createdAt = createdAt;
            _updatedAt = updatedAt;
        }

        public int Id { get { return _id; } }
        public DateTimeOffset CreatedAt { get { return _createdAt; } }
        public DateTimeOffset Updated_at { get { return _updatedAt; } }

        public void UpdateUser()
        {
            _updatedAt = new DateTimeOffset(DateTime.Now);
        }
    }
}